using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Barrel : DestructibleObject, IHitable, IShotable, ICutable, IExplosive
{
    [SerializeField] private float _forceBlastWave = 500f;
    [SerializeField] private float _radiusShockWave = 3.8f;
    [SerializeField] private float _radiusBurnWave = 2.15f;
    [SerializeField] private float _startDelay = 1.5f;
    [SerializeField, MinMaxSlider(0.0f, 5.0f)] private Vector2 _explosionDelay = new Vector2(0.5f,0.7f);
    [SerializeField] private float _burnOffset = 0.316f;
    [SerializeField] private LayerMask _blastWaveMask;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private AnimationCurve _extinctionBlastWaveCurve;

    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private CancellationTokenSource _cancellationTokenSource;
    private BarrelPool _barrelPoolEffects;
    private BlastWave _blastWave;
    private BarrelAudioHandler _barrelAudioHandler;
    private IGlobalAudio _globalAudio;
    private AudioClipProvider _audioClipProvider;
    private TimeScaleSignal _timeScaleSignal;
    private ExplodeSignal _explodeSignal;
    private ILevel _level;

    private Collider2D _collider2D => WholeObjectTransform.GetComponent<Collider2D>();
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;
    public Vector2 Position => TransformBase.position;
    public bool IsBroken => base.ObjectIsBroken;
    public Transform TargetTransform => TransformBase;

    [Inject]
    private void Construct(ILevel level, IGlobalAudio globalAudio, AudioClipProvider audioClipProvider, ExplodeSignal explodeSignal, TimeScaleSignal timeScaleSignal)
    {
        _barrelPoolEffects = level.LevelPool.BarrelPool;
        _globalAudio = globalAudio;
        _audioClipProvider = audioClipProvider;
        _timeScaleSignal = timeScaleSignal;
        _explodeSignal = explodeSignal;
        _level = level;
    }

    private void Awake()
    {
        _barrelAudioHandler = new BarrelAudioHandler(GetComponent<AudioSource>(),
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty, new TimeScalePitchHandler(_timeScaleSignal),
            _audioClipProvider.LevelAudioClipProvider.HitBarrelAudioClip,
            _audioClipProvider.LevelAudioClipProvider.Explode1BarrelAudioClip,
            _audioClipProvider.LevelAudioClipProvider.Explode2BarrelAudioClip,
            _audioClipProvider.LevelAudioClipProvider.BurnBarrelAudioClip);
        
        _blastWave = new BlastWave(_level.LevelPool.DebrisPool, WholeObjectTransform, _extinctionBlastWaveCurve, _blastWaveMask,
            _radiusShockWave, _radiusBurnWave, _forceBlastWave);
        
        base.Init(_level.LevelAudio.DebrisAudioHandler.PlayDebrisHitSound);
    }

    public void DestructFromCut(Vector2 cutPos)
    {
        if (ObjectIsBroken == false)
        {
            DestructAndExplosion();
        }
    }

    public void DestructFromShoot(Vector2 force)
    {
        if (ObjectIsBroken == false)
        {
            DestructAndExplosion();
        }
    }

    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (forceHit > Hardness)
            {
                DestructAndExplosion();
                result = true;
            }
            else
            {
                _barrelAudioHandler.PlayBarrelFailBreakingSound(forceHit);
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    public bool TryBreakOnExplosion(Vector2 direction, float forceHit)
    {
        if (IsBroken == false)
        {
            Rigidbody2D.AddForce(direction * forceHit);
            if (forceHit > Hardness)
            {
                ObjectIsBroken = true;
                float delay = UnityEngine.Random.Range(_explosionDelay.x, _explosionDelay.y);
                _barrelPoolEffects.PlayBarrelBurnEffect(new Vector2(WholeObjectTransform.position.x,
                        WholeObjectTransform.position.y + _burnOffset), delay);
                _barrelAudioHandler.PlayBarrelBurn(delay).Forget();
                DelayAndDo(DestructAndExplosion, UnityEngine.Random.Range(_explosionDelay.x, _explosionDelay.y)).Forget();
            }
            else
            {
                _barrelAudioHandler.PlayBarrelFailBreakingSound(forceHit);
            }
        }
        return false;
    }

    private void DestructAndExplosion()
    {
        Destruct();
        Explosion();
    }
    private void Explosion()
    {
        _barrelAudioHandler.PlayBarrelExplosionSound();
        _blastWave.InteractWithBlastWave();
        _barrelPoolEffects.PlayBarrelExplosionEffect(WholeObjectTransform.position);
        _explodeSignal.OnExplosion?.Invoke(WholeObjectTransform.PositionVector2());
    }
    private void SubscribeCheckCollision()
    {
        DelayAndDo(() =>
        {
            _collider2D.OnCollisionEnter2DAsObservable()
                .Subscribe(HandleCollision).AddTo(_compositeDisposable);
        }, _startDelay).Forget();
    }
    private void HandleCollision(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & _groundMask.value) == 1 << collision.gameObject.layer)
        {
            float impulse = 0f;
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (collision.contacts[i].normalImpulse > impulse)
                {
                    impulse = collision.contacts[i].normalImpulse;
                }
            }
            TryBreakOnImpact(impulse);
        }
    }
    private async UniTaskVoid DelayAndDo(Action operation, float time)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _cancellationTokenSource.Token);
        operation.Invoke();
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(WholeObjectTransform.position, _radiusShockWave);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(WholeObjectTransform.position, _radiusBurnWave);
        }
    }
    private new void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        SubscribeCheckCollision();
        base.OnEnable();
    }
    private void OnDisable()
    {
        _compositeDisposable?.Clear();
        _cancellationTokenSource?.Cancel();
        _barrelAudioHandler?.Dispose();
        _blastWave?.Dispose();
    }
}