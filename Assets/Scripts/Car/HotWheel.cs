using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class HotWheel
{
    public readonly HotWheelRef HotWheelRef;
    private readonly float _delayDisposeSlit = 2f;
    private readonly float _torqueMin = -30f;
    private readonly float _torqueMax = -100f;
    private readonly float _hotWheelRotationSpeed;
    private readonly float _distance = 0.05f;
    private readonly float _radiusWheel1;
    private readonly float _radiusWheel2;
    private readonly LayerMask _contactMask;
    private readonly int _layerAfterBreaking;
    private readonly HotWheelAudioHandler _hotWheelAudioHandler;
    private readonly Transform _debrisParent;
    private readonly Transform _wheel1;
    private readonly Transform _wheel2;
    private RaycastHit2D _wheel1Hit;
    private RaycastHit2D _wheel2Hit;
    private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    // private Rigidbody2D _wheel1Rigidbody2D;
    // private Rigidbody2D _wheel2Rigidbody2D;
    // private CircleCollider2D _wheel1Collider;
    // private CircleCollider2D _wheel2Collider;
    private List<Collider2D> _colliders = new List<Collider2D>(50);
    private bool _isBroken = false;
    private bool _circleCastOn = true;
    public HotWheel(HotWheelRef hotWheelRef, HotWheelAudioHandler hotWheelAudioHandler, Transform debrisParent, LayerMask contactMask, int layerAfterBreaking,
        float hotWheelRotationSpeed, float radiusWheel1, float radiusWheel2)
    {
        HotWheelRef = hotWheelRef;
        _hotWheelAudioHandler = hotWheelAudioHandler;
        _debrisParent = debrisParent;
        _hotWheelRotationSpeed = hotWheelRotationSpeed;
        _radiusWheel1 = radiusWheel1;
        _radiusWheel2 = radiusWheel2;
        _contactMask = contactMask;
        _layerAfterBreaking = layerAfterBreaking;
        _wheel1 = hotWheelRef.Wheel1;
        _wheel2 = hotWheelRef.Wheel2;
        // _wheel1Collider = hotWheelRef.Wheel1Collider;
        // _wheel2Collider = hotWheelRef.Wheel2Collider;
        _hotWheelAudioHandler.PlayRotateWheels();
    }
    public void Dispose()
    {
        _cancellationToken.Cancel();
    }

    public void Destruct()
    {
        _isBroken = true;
        HotWheelRef.transform.SetParent(_debrisParent);
        HotWheelRef.Wheel1.gameObject.AddComponent<Rigidbody2D>().AddTorque(GetRandomTorque());
        HotWheelRef.Wheel2.gameObject.AddComponent<Rigidbody2D>().AddTorque(GetRandomTorque());
        // _wheel1Rigidbody2D = HotWheelRef.Wheel1.gameObject.AddComponent<Rigidbody2D>();
        // _wheel2Rigidbody2D = HotWheelRef.Wheel2.gameObject.AddComponent<Rigidbody2D>();
        // _wheel1Rigidbody2D.AddTorque(GetRandomTorque());
        // _wheel2Rigidbody2D.AddTorque(GetRandomTorque());
        Physics2D.IgnoreLayerCollision(
            HotWheelRef.Wheel1.gameObject.layer, 
            _layerAfterBreaking,
            false);
        _hotWheelAudioHandler.StopPlaySoundRotateWheels().Forget();
        DelayCircleCastOff().Forget();
    }

    public void Update()
    {
        if (_isBroken == false)
        {
            _wheel1.Rotate(Vector3.forward, _hotWheelRotationSpeed);
            _wheel2.Rotate(Vector3.forward, _hotWheelRotationSpeed);
        }

        if (_circleCastOn == true)
        {
            CheckContact();
        }
    }
    private async UniTaskVoid DelayCircleCastOff()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDisposeSlit),  cancellationToken: _cancellationToken.Token);
        _circleCastOn = false;
    }
    private void CheckContact()
    {
        _wheel1Hit = Physics2D.CircleCast(_wheel1.position, _radiusWheel1,
            Vector2.right, _distance, _contactMask.value);
        _wheel2Hit = Physics2D.CircleCast(_wheel2.position, _radiusWheel2,
            Vector2.right, _distance, _contactMask.value);
        
        TryCut(ref _wheel1Hit);
        TryCut(ref _wheel2Hit);
    }
    private void TryCut(ref RaycastHit2D wheelHit)
    {
        if (wheelHit == true && wheelHit.collider.transform.parent.TryGetComponent(out ICutable cutable))
        {
            cutable.DestructFromCut(wheelHit.point);
            _hotWheelAudioHandler.TryPlayCut();
        }
    }

    private float GetRandomTorque()
    {
        return Random.Range(_torqueMin, _torqueMax);
    }
}