using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using System;
using NaughtyAttributes;

public class Bot : MonoBehaviour, IHitable, IShotable, IExplodable
{
    private const float DEATH_VALUE = 50f;
    [SerializeField, BoxGroup("Vision")] private LayerMask _visionLayer;
    [Space]
    [SerializeField, BoxGroup("Body")] private IKManager2D _skeleton;
    [SerializeField, BoxGroup("Body")] private Transform _targetCalm;
    [SerializeField, BoxGroup("Body")] private Transform _head;
    [SerializeField, BoxGroup("Body")] private Transform _visionPoint;
    [SerializeField, BoxGroup("Body")] private Transform _body;
    [Space]
    [SerializeField, BoxGroup("GunAttachmentPoints")] private Transform _shoulderSupport;
    [SerializeField, BoxGroup("GunAttachmentPoints")] private Transform _leftHand;
    [SerializeField, BoxGroup("GunAttachmentPoints")] private Transform _rightHand;
    [Space]
    [SerializeField, BoxGroup("Stats")] private float _radiusDetection = 15f;
    [SerializeField, BoxGroup("Stats")] private float _rangeVision = 20f;
    [SerializeField, BoxGroup("Stats")] private float _speedLookAt = 5f;
    [SerializeField, BoxGroup("Stats")] private float _timeRagdollActive = 3f;
    [SerializeField, BoxGroup("Stats")] private bool _isFlipped;
    [SerializeField, BoxGroup("Stats")] private bool _targetDetected;

    [SerializeField] private Gun _gunPrefab;
    [Space]
    [SerializeField, BoxGroup("AudioClips")] private AudioClip _botSaluteAudioClip;
    [SerializeField, BoxGroup("AudioClips")] private AudioClip _botDieAudioClip;
    [SerializeField, BoxGroup("AudioClips")] private AudioClip _botBueAudioClip;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _capsuleCollider;
    private LookAt _lookAt;
    private Flip _flipBot;
    private Vision _vision;
    private Gun _gun;
    private BotRagdoll _botRagdoll;
    private BotSound _botSound;
    private KillsCount _killsCount;
    //private int _indexForRagdollRigidBody;
    public bool IsLive { get; private set; }
    public bool IsLive2 { get; private set; }
    public Transform TargetTransform { get; private set; }
    public event Action OnDestruct;
    private bool _inShelter;

    public void Init(Transform target, BulletReservoir bulletReservoir, KillsCount killsCount, bool inShelter = false)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        InitTargetTransform();
        _killsCount = killsCount;
        _inShelter = inShelter;
        CheckShelter();
        IsLive = true;
        IsLive2 = true;
        var gunOnject = Instantiate(_gunPrefab, transform);
        _gun = gunOnject.GetComponent<Gun>();
        _botSound = new BotSound(GetComponent<AudioSource>(), _botSaluteAudioClip, _botBueAudioClip, _botDieAudioClip);
        _botRagdoll = GetComponentInChildren<BotRagdoll>();
        _vision = new Vision( _visionLayer, _visionPoint, target, _targetCalm, _radiusDetection, _botSound);
        _flipBot = new Flip(target, transform, _vision);
        _lookAt = new LookAt(_flipBot, _vision, _gun.transform, _head, _body, _speedLookAt);

        _gun.Init(_shoulderSupport, _leftHand, _rightHand, bulletReservoir, _botSound);
        _botRagdoll.Init(_gun, _skeleton.gameObject, _timeRagdollActive);
        _botRagdoll.OnDestroy += DestroyGameObject;
    }
    private void InitTargetTransform()
    {
        var _indexForRagdollRigidBody = UnityEngine.Random.Range(0, 2);

        if (_indexForRagdollRigidBody == 0)
        {
            TargetTransform = _head;
        }
        else
        {
            TargetTransform = _body;
        }
    }
    public void UpdateBot()
    {
        if (IsLive == true)
        {
            _vision.Update();
            _flipBot.CheckFlip();
            _lookAt.Update();
            _gun.UpdateGun(IsLive, _vision.TargetDetected, _flipBot.IsFliped);
        }

        //else if (IsLive2 == true)
        //{
        //    _gun.UpdateGun(false, false , _flipBot.IsFliped);
        //    //_lookAt.FixedUpdate();

        //}
        _isFlipped = _flipBot.IsFliped;
        _targetDetected = _vision.TargetDetected;
        _vision.RangeVision = _rangeVision;
    }
    private void CheckShelter()
    {
        if (_inShelter == true)
        {
            _rigidbody.isKinematic = true;
            _capsuleCollider.enabled = false;
        }
    }
    private void KillByCar()
    {
        Kill();
        _botRagdoll.ActiveRagdoll();
    }
    private void KillByGun(Vector2 force)
    {
        _rigidbody.freezeRotation = false;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForceAtPosition(force, TargetTransform.position);
        IsLive = false;
        _botSound.PlayDie();
        _gun.DropWeapon();
        _killsCount.AddKill();
        OnDestruct?.Invoke();
        StartCoroutine(DelayEnableRagdoll());
    }
    private void Kill()
    {
        IsLive = false;
        _capsuleCollider.enabled = false;
        OnDestruct?.Invoke();
        _killsCount.AddKill();
        _botSound.PlayDie();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_visionPoint.position, _radiusDetection);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(   _visionPoint.position, _rangeVision);
    }
    private void DestroyGameObject()
    {
        gameObject.SetActive(false);
        _botRagdoll.OnDestroy -= DestroyGameObject;
        Destroy(gameObject);
    }
    private bool CheckLive()
    {
        if (IsLive == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Vector2 Position { get; }
    public bool IsBroken { get; }
    public IReadOnlyList<DebrisFragment> DebrisFragments { get; }

    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if ((CheckLive() == false))
        {
            result = false;
        }
        else 
        {
            if (forceHit > DEATH_VALUE)
            {
                KillByCar();
                result = true;
            }
            result = false;
        }

        return result;
    }

    public void AddForce(Vector2 force)
    {
        
    }

    public void DestructFromShoot(Vector2 force)
    {
        if (CheckLive() == false)
        {
            return;
        }
        else
        {
            KillByGun(force);
        }
    }
    public void DestructByExplode()
    {
        Kill();
        _botRagdoll.ActiveRagdoll();
    }
    private IEnumerator DelayEnableRagdoll()
    {
        yield return new WaitForSeconds(0.2f);
        //Kill();
        //IsLive2 = false;
        _botRagdoll.ActiveRagdoll();
        _capsuleCollider.enabled = false;

    }
}
