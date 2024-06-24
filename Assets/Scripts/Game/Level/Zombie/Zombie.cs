using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.U2D.IK;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Zombie : MonoBehaviour, IHitable, IExplosive, IShotable, ICutable
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private Rigidbody2D _rigidbody2D;
    [SerializeField] private CapsuleCollider2D _collider2D;
    [SerializeField] private ZombieDirection _direction;
    [SerializeField, Range(0f,1f)] private float _speed = 0.4f;
    [SerializeField] private float _forceTearingUp;
    [SerializeField, Range(0f,1f)] private float _radiusSphereCast;
    [SerializeField, Range(-1f,1f)] private float _offsetXSphereCast;
    [SerializeField, Range(-1f,1f)] private float _offsetYSphereCast;
    [SerializeField] private LayerMask _contactMask;
    [SerializeField, Layer] private int _zombieDebrisLayer;
    [SerializeField, Range(1f,5.5f)] private float _zombieTalkSoundPeriod;

    [SerializeField, HorizontalLine(color: EColor.Orange), BoxGroup("Ragdoll")] private HingeJoint2D[] _hingeJoints2D;
    [SerializeField, BoxGroup("Ragdoll")] private Collider2D[] _colliders2D;
    [SerializeField, BoxGroup("Ragdoll")] private Rigidbody2D[] _rigidbodies2D;
    [SerializeField, BoxGroup("Ragdoll")] private IKManager2D _ikManager;
    [SerializeField, BoxGroup("Ragdoll")] private Animation _animation;
    [SerializeField, HorizontalLine(color: EColor.Red), BoxGroup("forTearingUp")] private HingeJoint2D[] _hingeJoints2DForTearingUp;
    [SerializeField, BoxGroup("forTearingUp")] private Transform[] _forTearingUp;
    [SerializeField, HorizontalLine(color: EColor.Red), BoxGroup("BloodPoints")] private Transform[] _bloodPoints;
    [SerializeField, BoxGroup("Head"), HorizontalLine(color: EColor.Indigo)] private HingeJoint2D _headHingeJoint2D;
    [SerializeField, BoxGroup("Head")] private Rigidbody2D _headRigidbody2D;
    [SerializeField, BoxGroup("Head")] private ParticleSystem _bloodFall;
    [SerializeField, BoxGroup("Body")] private Rigidbody2D _bodyRigidbody2D;
    private readonly float _delayChangeLayer = 1f;
    private readonly float _forceTearingUpMultiplier = 0.5f;
    private readonly float _addXRange = 20f;
    private Transform _transform;
    private Transform _cameraTransform;
    private ZombiePool _zombiePool;
    private GamePause _gamePause;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private Collision2D _collision2D;
    private ZombieMove _zombieMove;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private CancellationTokenSource _cancellationTokenSourceForTalkSound = new CancellationTokenSource();
    private GameObject[] _bodyParts;
    private List<DebrisFragment> _debrisFragments;
    private ZombieAudioHandler ZombieAudioHandler;
    protected event Action OnBroken;
    public Vector2 Position  => _transform.position;
    public IReadOnlyList<DebrisFragment> DebrisFragments => _debrisFragments;

    public bool IsBroken => IsBrokenReactiveProperty.Value;
    public ReactiveProperty<bool> IsBrokenReactiveProperty = new ReactiveProperty<bool>();
    public Transform TargetTransform { get; private set; }
    private Transform _headTransform;
    
    [Inject]
    private void Construct(GamePause gamePause, AudioClipProvider audioClipProvider, IGlobalAudio globalAudio,  ILevel level)
    {
        _cameraTransform = level.CameraTransform;
        _zombiePool = level.LevelPool.ZombiePool;
        TargetTransform = _bodyRigidbody2D.transform;
        _gamePause = gamePause;
        ZombieAudioHandler = new ZombieAudioHandler(GetComponent<AudioSource>(),
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty, audioClipProvider.LevelAudioClipProvider);
    }

    private void Awake()
    {
        _bodyParts = new GameObject[_rigidbodies2D.Length];
        for (int i = 0; i < _rigidbodies2D.Length; i++)
        {
            _bodyParts[i] = _rigidbodies2D[i].gameObject;
        }
        
        _transform = transform;
        _debrisFragments = new List<DebrisFragment>(_forTearingUp.Length);
        for (int i = 0; i < _forTearingUp.Length; i++)
        {
            DebrisFragment debrisFragment = _forTearingUp[i].gameObject.AddComponent<DebrisFragment>();
            debrisFragment.Init();
            debrisFragment.InitRigidBody();
            _debrisFragments.Add(debrisFragment);
        }
        _zombieMove = new ZombieMove(transform, _rigidbody2D, _gamePause, IsBrokenReactiveProperty, _contactMask,
                    new Vector2(_offsetXSphereCast, _offsetYSphereCast), (float)_direction, _speed, _radiusSphereCast);
        gameObject.SetActive(false);

        SubscribeEnableAndDisableObserve();
    }

    public void DestructFromShoot(Vector2 force)
    {
        if (IsBroken == false)
        {
            EnableRagdoll();
            ZombieAudioHandler.PlayDeath();
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                _headRigidbody2D.AddForce(force);
                _zombiePool.PlayBloodHitEffect(_headTransform.position);
                _zombiePool.PlayBloodEffect(_headTransform);

                _headHingeJoint2D.enabled = false;
            }
            else
            {
                _bodyRigidbody2D.AddForce(force);
                _zombiePool.PlayBloodHitEffect(_bodyRigidbody2D.transform.position);
            }
        }
    }

    public bool TryBreakOnExplosion(Vector2 direction, float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            ZombieAudioHandler.PlayHit();
            if (forceHit > _forceTearingUp)
            {
                EnableRagdoll();
                TearingUpZombie();
                ZombieAudioHandler.PlayDeath();
                result = true;
            }
            else if(forceHit > _forceTearingUp * _forceTearingUpMultiplier)
            {
                EnableRagdoll();
                ZombieAudioHandler.PlayDeath();
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }
    public bool TryBreakOnImpact(float forceHit)
    {
        if (IsBroken == false)
        {
            EnableRagdoll();

            if (forceHit > _forceTearingUp)
            {
                TearingUpZombie();
                AddBlood();
            }

            ZombieAudioHandler.PlayHit();
            ZombieAudioHandler.PlayDeath();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestructFromCut(Vector2 cutPos)
    {
        if (IsBroken == false)
        {
            EnableRagdoll();
            TearingUpZombie();
            _zombiePool.PlayBloodHitEffect(cutPos);
            AddBlood();
            ZombieAudioHandler.PlayDeath();

        }
    }

    private void AddBlood()
    {
        for (int i = 0; i < _bloodPoints.Length; i++)
        {
            _zombiePool.PlayBloodEffect(_bloodPoints[i]);
        }
    }
    private void TearingUpZombie()
    {
        foreach (var joint2D in _hingeJoints2DForTearingUp)
        {
            joint2D.enabled = false;
        }
    }
    private void EnableRagdoll()
    {
        IsBrokenReactiveProperty.Value = true;
        _bloodFall.Stop();
        _rigidbody2D.simulated = false;
        _collider2D.enabled = false;
        _ikManager.enabled = false;
        _animation.enabled = false;
        foreach (var rigidbody2D in _rigidbodies2D)
        {
            rigidbody2D.simulated = true;
        }

        foreach (var joint2D in _hingeJoints2D)
        {
            joint2D.enabled = true;
        }

        foreach (var collider2D in _colliders2D)
        {
            collider2D.enabled = true;
        }
        OnBroken?.Invoke();
        // SwitchLayerDelay().Forget();
        StartCoroutine(SwitchLayer());
        _cancellationTokenSourceForTalkSound.Cancel();
    }

    // private async UniTaskVoid SwitchLayerDelay()
    // {
    //     await UniTask.Delay(TimeSpan.FromSeconds(_delayChangeLayer), cancellationToken: _cancellationTokenSource.Token);
    //     foreach (var bodyPart in _bodyParts)
    //     {
    //         bodyPart.layer = _zombieDebrisLayer;
    //     }
    // }
    private IEnumerator SwitchLayer()
    {
        yield return new WaitForSeconds(_delayChangeLayer);
        foreach (var bodyPart in _bodyParts)
        {
            bodyPart.layer = _zombieDebrisLayer;
        }
    }
    private void SubscribeEnableAndDisableObserve()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (TargetTransform.position.x  + _addXRange < _cameraTransform.position.x)
            {
                gameObject.SetActive(false);
            }
            
            else if(TargetTransform.position.x   < _cameraTransform.position.x + _addXRange)
            {
                gameObject.SetActive(true);
            }
        }).AddTo(_compositeDisposable);
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.PositionVector2() + new Vector2(_offsetXSphereCast, _offsetYSphereCast), _radiusSphereCast);
        }
    }
    protected async UniTaskVoid StartCyclePlaySound(CancellationTokenSource cancellationTokenSource, Action operation, float startDelay, float nextDelay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(startDelay), cancellationToken: cancellationTokenSource.Token);
        while (true)
        {
            operation?.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(nextDelay), cancellationToken: cancellationTokenSource.Token);
        }
    }
    private void OnEnable()
    {
        _animation.Play();
        _bloodFall.Play();
        StartCyclePlaySound(_cancellationTokenSourceForTalkSound, () => {ZombieAudioHandler.PlayTalk();},
            Random.Range(1f, _zombieTalkSoundPeriod), _zombieTalkSoundPeriod).Forget();
    }

    private void OnDisable()
    
    {
        _animation.Stop();
        _bloodFall.Stop();
        _cancellationTokenSourceForTalkSound.Cancel();
    }

    private void OnDestroy()
    {
        _compositeDisposable.Clear();
        if (_debrisFragments != null)
        {
            for (int i = 0; i < _debrisFragments.Count; i++)
            {
                _debrisFragments[i].Dispose();
            }

            _debrisFragments = null;
        }
    }
}
