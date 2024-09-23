using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class HotWheel
{
    public readonly HotWheelRef HotWheelRef;
    private readonly float _delayDisposeSlit = 2f;
    private readonly float _torqueMin = -30f;
    private readonly float _torqueMax = -100f;
    private readonly float _hotWheelRotationSpeed;
    private readonly float _distance = 0.1f;
    private readonly float _radiusWheel2;
    private readonly LayerMask _contactMask;
    private readonly Vector3 _offset;
    private readonly int _layerAfterBreaking;
    private readonly HotWheelAudioHandler _hotWheelAudioHandler;
    private readonly DebrisKeeper _debrisKeeper;
    private readonly Transform _wheel1;
    private readonly Transform _wheel2;
    private RaycastHit2D _wheelHit;
    private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    private Rigidbody2D _wheel1Rigidbody2D;
    private Rigidbody2D _wheel2Rigidbody2D;
    private CircleCollider2D _wheel1Collider;
    private CircleCollider2D _wheel2Collider;
    private List<Collider2D> _colliders = new List<Collider2D>(50);
    private bool _isBroken = false;
    private bool _circleCastOn = true;
    public HotWheel(HotWheelRef hotWheelRef, HotWheelAudioHandler hotWheelAudioHandler, DebrisKeeper debrisKeeper, LayerMask contactMask, Vector3 offset,
        int layerAfterBreaking, float hotWheelRotationSpeed, float radiusWheel2)
    {
        HotWheelRef = hotWheelRef;
        _hotWheelAudioHandler = hotWheelAudioHandler;
        _debrisKeeper = debrisKeeper;
        _hotWheelRotationSpeed = hotWheelRotationSpeed;
        _radiusWheel2 = radiusWheel2;
        _contactMask = contactMask;
        _offset = offset;
        _layerAfterBreaking = layerAfterBreaking;
        _wheel1 = hotWheelRef.Wheel1;
        _wheel2 = hotWheelRef.Wheel2;
        _wheel1Collider = hotWheelRef.Wheel1Collider;
        _wheel2Collider = hotWheelRef.Wheel2Collider;
        _wheel1Rigidbody2D = hotWheelRef.Rigidbody2DWheel1;
        _wheel2Rigidbody2D = hotWheelRef.Rigidbody2DWheel2;
        _wheel1Rigidbody2D.simulated = false;
        _wheel2Rigidbody2D.simulated = false;
        _hotWheelAudioHandler.PlayRotateWheels();
    }
    public void Dispose()
    {
        _cancellationToken.Cancel();
    }

    public void Destruct()
    {
        _isBroken = true;
        _debrisKeeper.AddDebris(HotWheelRef.transform);
        _wheel1Collider.enabled = true;
        _wheel2Collider.enabled = true;
        _wheel1Rigidbody2D.simulated = true;
        _wheel2Rigidbody2D.simulated = true;
        _wheel1Rigidbody2D.AddTorque(GetRandomTorque());
        _wheel2Rigidbody2D.AddTorque(GetRandomTorque());
        Physics2D.IgnoreLayerCollision(
            HotWheelRef.Wheel1.gameObject.layer, 
            _layerAfterBreaking,
            false);
        _hotWheelAudioHandler.StopPlaySoundRotateWheels().Forget();
        DelayCircleCastOff().Forget();
    }
    public void FixedUpdate()
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
        _wheelHit = Physics2D.CircleCast(_wheel2.position + _offset, _radiusWheel2,
            Vector2.right, _distance, _contactMask.value);
        if (_wheelHit == true
            && _wheelHit.collider.transform.parent.TryGetComponent(out ICutable cutable))
        {
            cutable.DestructFromCut(_wheelHit.point);
            _hotWheelAudioHandler.TryPlayCut();
        }
    }

    private float GetRandomTorque()
    {
        return Random.Range(_torqueMin, _torqueMax);
    }
}