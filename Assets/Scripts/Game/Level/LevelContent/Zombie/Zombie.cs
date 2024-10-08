using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.U2D.IK;
using Zenject;

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
    private readonly float _volumeDefaultValue = 1f;
    private int _zombieDebrisLayer;
    private int _layerCar;
    
    private Transform _transform;
    private ZombiePool _zombiePool;
    private GamePause _gamePause;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private CancellationTokenSource _cancellationTokenSource;
    private Collision2D _collision2D;
    private ZombieMove _zombieMove;
    private ZombieAudioHandler _zombieAudioHandler;
    private WaitForSeconds _waitForSeconds;
    private KillsSignal _killsSignal;
    
    private GameObject[] _bodyParts;
    private List<DebrisFragment> _debrisFragments;

    protected GameOverSignal GameOverSignal;
    protected IGlobalAudio GlobalAudio;
    protected AudioClipProvider AudioClipProvider;
    protected event Action OnBroken;
    
    public Vector2 Position  => _transform.position;
    public IReadOnlyList<DebrisFragment> DebrisFragments => GetAndActivateDebrisFragments();

    public bool IsBroken { get; private set; }
    public Transform TargetTransform { get; private set; }
    
    [Inject]
    private void Construct(GamePause gamePause, AudioClipProvider audioClipProvider, LayersProvider layersProvider,
        KillsSignal killsSignal, GameOverSignal gameOverSignal, IGlobalAudio globalAudio,  ILevel level)
    {
        _zombiePool = level.LevelPool.ZombiePool;
        TargetTransform = _bodyRigidbody2D.transform;
        _gamePause = gamePause;
        _killsSignal = killsSignal;
        _zombieDebrisLayer = layersProvider.ZombieDebrisLayer;
        _layerCar = layersProvider.CarLayer;
        GlobalAudio = globalAudio;
        AudioClipProvider = audioClipProvider;
        GameOverSignal = gameOverSignal;
    }

    protected void Awake()
    {
        _zombieAudioHandler = new ZombieAudioHandler(GetComponent<AudioSource>(),
            GlobalAudio.SoundReactiveProperty, GlobalAudio.AudioPauseReactiveProperty, AudioClipProvider.LevelAudioClipProvider);
        GameOverSignal.OnGameOver += OnGameOverSignal;
        
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
            debrisFragment.Init(_zombieAudioHandler.PlayHit, _zombieDebrisLayer, _layerCar);
            _debrisFragments.Add(debrisFragment);
        }
        _zombieMove = new ZombieMove(transform, _rigidbody2D, _gamePause, _contactMask,
                    new Vector2(_offsetXSphereCast, _offsetYSphereCast), (float)_direction, _speed, _radiusSphereCast);
    }

    protected void OnEnable()
    {
        _animation.Play();
        _bloodFall.Play();
        _zombieMove.Init();
        _cancellationTokenSource = new CancellationTokenSource();
        _zombieAudioHandler.StartCyclePlaySound(_zombieAudioHandler.PlayTalk, _zombieTalkSoundPeriod);
    }

    protected void OnDisable()
    {
        GameOverDisable();
    }
    
    public void DestructFromShoot(Vector2 force)
    {
        if (IsBroken == false)
        {
            EnableRagdoll();
            _zombieAudioHandler.PlayDeath();
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                _headRigidbody2D.AddForce(force);
                _zombiePool.PlayBloodHitEffect(_headRigidbody2D.position);
                _zombiePool.PlayBloodEffect(_headRigidbody2D.transform);

                _headHingeJoint2D.enabled = false;
            }
            else
            {
                _bodyRigidbody2D.AddForce(force);
                _zombiePool.PlayBloodHitEffect(_bodyRigidbody2D.position);
            }
        }
    }

    public bool TryBreakOnExplosion(Vector2 direction, float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            _zombieAudioHandler.PlayHit(_volumeDefaultValue);
            if (forceHit > _forceTearingUp)
            {
                EnableRagdoll();
                TearingUpZombie();
                _zombieAudioHandler.PlayDeath();
                result = true;
            }
            else if(forceHit > _forceTearingUp * _forceTearingUpMultiplier)
            {
                EnableRagdoll();
                _zombieAudioHandler.PlayDeath();
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

            _zombieAudioHandler.PlayHit(_volumeDefaultValue);
            _zombieAudioHandler.PlayDeath();
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
            _zombieAudioHandler.PlayDeath();

        }
    }

    private IReadOnlyList<DebrisFragment> GetAndActivateDebrisFragments()
    {
        ActivateDebrisFragments();
        return _debrisFragments;
    }

    private void ActivateDebrisFragments()
    {
        foreach (DebrisFragment debrisFragment in _debrisFragments)
        {
            debrisFragment.Activate();
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
        IsBroken = true;
        _killsSignal.OnKill?.Invoke();
        ZombieBroken();
        // _bloodFall.Stop();
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

        ActivateDebrisFragments();
        OnBroken?.Invoke();
        SwitchLayer().Forget();
    }
    private async UniTaskVoid SwitchLayer()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayChangeLayer), cancellationToken: _cancellationTokenSource.Token);
        foreach (var bodyPart in _bodyParts)
        {
            bodyPart.layer = _zombieDebrisLayer;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.PositionVector2() + new Vector2(_offsetXSphereCast, _offsetYSphereCast), _radiusSphereCast);
        }
    }

    private void OnGameOverSignal()
    {
        if (gameObject.activeInHierarchy == true)
        {
            GameOverDisable();
        }
    }

    private void ZombieBroken()
    {
        _animation.Stop();
        _bloodFall.Stop();
        _zombieMove?.Dispose();
    }

    private void GameOverDisable()
    {
        ZombieBroken();
        _zombieAudioHandler.StopCyclePlaySound();
        _cancellationTokenSource.Cancel();
    }

    private void OnDestroy()
    {
        _compositeDisposable.Clear();
        if (GameOverSignal != null)
        {
            GameOverSignal.OnGameOver -= OnGameOverSignal;
        }

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
