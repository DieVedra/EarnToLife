using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Barrel : DestructibleObject, IHitable, IShotable, ICutable, IExplosive
{
    [SerializeField] private float _forceBlastWave;
    [SerializeField] private float _radiusShockWave;
    [SerializeField] private float _radiusBurnWave;
    [SerializeField] private float _startDelay = 1.5f;
    [SerializeField, MinMaxSlider(0.0f, 5.0f)] private Vector2 _explosionDelay;
    [SerializeField] private float _burnOffset = 0.316f;
    [SerializeField] private LayerMask _blastWaveMask;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private AnimationCurve _extinctionBlastWaveCurve;
    private BarrelPool _barrelPoolEffects;
    private BlastWave _blastWave;
    private BarrelAudioHandler _barrelAudioHandler;
    private ExplodeSignal _explodeSignal;
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private Collider2D _collider2D => WholeObjectTransform.GetComponent<Collider2D>();
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;
    public Vector2 Position => TransformBase.position;
    public bool IsBroken => base.ObjectIsBroken;
    public Transform TargetTransform => TransformBase;

    [Inject]
    private void Construct(ILevel level, IGlobalAudio globalAudio, AudioClipProvider audioClipProvider, ExplodeSignal explodeSignal, TimeScaleSignal timeScaleSignal)
    {
        _barrelPoolEffects = level.LevelPool.BarrelPool;
        _barrelAudioHandler = new BarrelAudioHandler(GetComponent<AudioSource>(),
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty, new TimeScalePitchHandler(timeScaleSignal),
            audioClipProvider.LevelAudioClipProvider.HitBarrelAudioClip,
            audioClipProvider.LevelAudioClipProvider.Explode1BarrelAudioClip,
            audioClipProvider.LevelAudioClipProvider.Explode2BarrelAudioClip,
            audioClipProvider.LevelAudioClipProvider.BurnBarrelAudioClip);
        DebrisHitSound = level.LevelAudio.DebrisAudioHandler.PlayDebrisHitSound;

        
        
        _explodeSignal = explodeSignal;
        _blastWave = new BlastWave(level.LevelPool.DebrisPool, WholeObjectTransform, _extinctionBlastWaveCurve, _blastWaveMask,
            _radiusShockWave, _radiusBurnWave, _forceBlastWave);
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
                StartCoroutine(ExplosionDelay(DestructAndExplosion));
            }
            else
            {
                _barrelAudioHandler.PlayBarrelFailBreakingSound(forceHit);
            }
        }
        return false;
    }

    public string Name => gameObject.name;

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
        _explodeSignal.OnExplosion?.Invoke();
    }
    private IEnumerator SubscribeCheckCollision()
    {
        yield return new WaitForSeconds(_startDelay);
        _collider2D.OnCollisionEnter2DAsObservable()
            .Do(HandleCollision)
            .Subscribe().AddTo(_compositeDisposable);
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

    private IEnumerator ExplosionDelay(Action operation)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_explosionDelay.x, _explosionDelay.y));
        operation?.Invoke();
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
        StartCoroutine(SubscribeCheckCollision());
        base.OnEnable();
    }
    private new void OnDisable()
    {
        StopCoroutine(SubscribeCheckCollision());
        // SubscribeCheckCollision()
        _compositeDisposable.Clear();
        _barrelAudioHandler.Dispose();
    }
}