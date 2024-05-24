using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.U2D.IK;
using Zenject;

public class Zombie : MonoBehaviour, IHitable, IExplosive, IShotable, ICutable
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private Rigidbody2D _rigidbody2D;
    [SerializeField] private CapsuleCollider2D _collider2D;
    [SerializeField] private ZombieDirection _direction;
    [SerializeField, Range(0f,1f)] private float _speed = 0.4f;
    [SerializeField] private float _forceTearingUp;
    [SerializeField] private LayerMask _contactMask;

    [SerializeField, HorizontalLine(color: EColor.Orange)] private HingeJoint2D[] _hingeJoints2D;
    [SerializeField] private Collider2D[] _colliders2D;
    [SerializeField] private Rigidbody2D[] _rigidbodies2D;
    [SerializeField] private IKManager2D _ikManager;
    [SerializeField] private Animation _animation;
    [SerializeField, HorizontalLine(color: EColor.Red)] private HingeJoint2D[] _hingeJoints2DForTearingUp;
    [SerializeField] private Rigidbody2D[] _rigidbodies2DForTearingUp;


    
    [SerializeField, BoxGroup("Head"), HorizontalLine(color: EColor.Indigo)] private HingeJoint2D _headHingeJoint2D;
    [SerializeField, BoxGroup("Head")] private Rigidbody2D _headRigidbody2D;
    [SerializeField, BoxGroup("Body")] private Rigidbody2D _bodyRigidbody2D;

    private readonly float _forceTearingUpMultiplier = 0.5f;
    private Transform _transform;
    private Transform _debrisParentForDestroy;
    private IGamePause _gamePause;
    private BloodPool _bloodPool;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private CompositeDisposable _compositeDisposableForUpdate = new CompositeDisposable();
    private Collision2D _collision2D;
    private ZombieAudioHandler _zombieAudioHandler;
    private ZombieMove _zombieMove;
    private List<DebrisFragment> _debrisFragments;

    public Vector2 Position  => _transform.position;
    public IReadOnlyList<DebrisFragment> DebrisFragments => _debrisFragments;

    public bool IsBroken { get; private set; }
    public Transform TargetTransform => _transform;

    [Inject]
    private void Construct(GamePause gamePause, ILevel level)
    {
        _gamePause = gamePause;
        _debrisParentForDestroy = level.DebrisParent;
        _zombieAudioHandler = level.LevelAudio.ZombieAudioHandler;
        _bloodPool = level.LevelPool.BloodPool;
        _zombieMove = new ZombieMove(transform, _rigidbody2D, _collider2D, gamePause, _contactMask, (float)_direction, _speed);
    }

    public void DestructFromShoot(Vector2 force)
    {
        if (IsBroken == false)
        {
            EnableRagdoll();
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                _headRigidbody2D.AddForce(force);
                _headHingeJoint2D.enabled = false;
            }
            else
            {
                _bodyRigidbody2D.AddForce(force);
            }
        }
    }

    public bool TryBreakOnExplosion(Vector2 direction, float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (forceHit > _forceTearingUp)
            {
                // _woodDestructibleAudioHandler.PlayWoodBreakingSound();
                EnableRagdoll();
                TearingUpZombie();
                result = true;
            }
            else if(forceHit > _forceTearingUp * _forceTearingUpMultiplier)
            {
                EnableRagdoll();
                result = true;
            }
            else
            {
                // _woodDestructibleAudioHandler.PlayWoodNotBreakingSound(forceHit);
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
                //add blood
                //add sound
            }
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
            //add blood
            //add sound

        }
    }

    private void TearingUpZombie()
    {
        foreach (var joint2D in _hingeJoints2DForTearingUp)
        {
            joint2D.enabled = false;
        }
    }
    [Button("EnableRagdoll")]
    private void EnableRagdoll()
    {
        IsBroken = true;
        _collider2D.enabled = false;
        _rigidbody2D.simulated = false;
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_collider2D.OriginLowerCirclePosition(transform), _collider2D.RadiusCircle());
    }

    private void FixedUpdate()
    {
        if (IsBroken == false)
        {
            _zombieMove.Update();
        }
    }

    private void OnEnable()
    {
        _transform = transform;
        _debrisFragments = new List<DebrisFragment>();

        for (int i = 0; i < _rigidbodies2DForTearingUp.Length; i++)
        {
            _debrisFragments.Add(new DebrisFragment(_rigidbodies2DForTearingUp[i]));
        }
        _animation.Play();
    }

    private void OnDisable()
    {
        _animation.Stop();
        _debrisFragments = null;
    }
}
