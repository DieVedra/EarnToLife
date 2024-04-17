using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class HotWheel
{
    public readonly HotWheelRef HotWheelRef;
    private readonly float _delayDisposeSlit = 2f;
    private readonly float _hotWheelRotationSpeed;
    private readonly float _distance = 0.05f;
    private readonly float _radiusWheel1;
    private readonly float _radiusWheel2;
    private readonly LayerMask _contactMask;
    private readonly ContactFilter2D _contactFilter;
    private readonly HotWheelAudioHandler _hotWheelAudioHandler;
    private readonly Transform _wheel1;
    private readonly Transform _wheel2;
    private RaycastHit2D _wheel1Hit;
    private RaycastHit2D _wheel2Hit;
    private CompositeDisposable _compositeDisposableRotate = new CompositeDisposable();
    private CompositeDisposable _compositeDisposableSlit = new CompositeDisposable();
    private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    private Rigidbody2D _wheel1Rigidbody2D;
    private Rigidbody2D _wheel2Rigidbody2D;
    private CircleCollider2D _wheel1Collider;
    private CircleCollider2D _wheel2Collider;
    private List<Collider2D> _colliders = new List<Collider2D>(50);
    private bool _isBroken = false;
    public HotWheel(HotWheelRef hotWheelRef, HotWheelAudioHandler hotWheelAudioHandler, LayerMask contactMask,
        float hotWheelRotationSpeed, float radiusWheel1, float radiusWheel2)
    {
        HotWheelRef = hotWheelRef;
        _hotWheelAudioHandler = hotWheelAudioHandler;
        _hotWheelRotationSpeed = hotWheelRotationSpeed;
        _radiusWheel1 = radiusWheel1;
        _radiusWheel2 = radiusWheel2;
        _contactMask = contactMask;
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(contactMask);
        _wheel1 = hotWheelRef.Wheel1;
        _wheel2 = hotWheelRef.Wheel2;
        _wheel1Collider = hotWheelRef.Wheel1Collider;
        _wheel2Collider = hotWheelRef.Wheel2Collider;
        // SubscribeUpdate();
        // SubscribeToSlit();
        _hotWheelAudioHandler.PlayRotateWheels();
    }
    public void Dispose()
    {
        _compositeDisposableRotate.Clear();
        _compositeDisposableSlit.Clear();
        _cancellationToken.Cancel();
    }

    public void Destruct()
    {
        // _compositeDisposableRotate.Clear();
        _isBroken = true;
        _wheel1Rigidbody2D = HotWheelRef.Wheel1.gameObject.AddComponent<Rigidbody2D>();
        _wheel2Rigidbody2D = HotWheelRef.Wheel2.gameObject.AddComponent<Rigidbody2D>();
        _wheel1Rigidbody2D.AddTorque(100f);
        _wheel2Rigidbody2D.AddTorque(100f);
        
        
        _hotWheelAudioHandler.StopPlayRotateWheels().Forget();
        DelayDisposeSlit().Forget();
    }

    public void Update()
    {
        if (_isBroken == false)
        {
            _wheel1.Rotate(Vector3.forward, _hotWheelRotationSpeed);
            _wheel2.Rotate(Vector3.forward, _hotWheelRotationSpeed);
            CheckContact();
        }
    }
    private async UniTaskVoid DelayDisposeSlit()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDisposeSlit),  cancellationToken: _cancellationToken.Token);
        _compositeDisposableSlit.Clear();
    }
    private void CheckContact()
    {
        _wheel1Hit = Physics2D.CircleCast(_wheel1.position, _radiusWheel1,
            Vector2.right, _distance, _contactMask.value);
        _wheel2Hit = Physics2D.CircleCast(_wheel2.position, _radiusWheel2,
            Vector2.right, _distance, _contactMask.value);
        
        TryCut(ref _wheel2Hit);
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
    // private void HandleColliders()
    // {
    //     for (int i = 0; i < _colliders.Count; i++)
    //     {
    //         if (_colliders[i].gameObject.TryGetComponent(out ICutable cutable))
    //         {
    //             cutable.DestructFromCut();
    //             _hotWheelAudioHandler.TryPlayCut();
    //         }
    //     }
    // }
    // private void SubscribeToSlit()
    // {
    //     _wheel1.GetComponent<Collider2D>().OnCollisionEnter2DAsObservable()
    //         .Do(Slit).Subscribe().AddTo(_compositeDisposableSlit);
    //     
    //     _wheel2.GetComponent<Collider2D>().OnCollisionEnter2DAsObservable()
    //         .Do(Slit).Subscribe().AddTo(_compositeDisposableSlit);
    // }
    // private void Slit(Collision2D collision)
    // {
    //     Debug.Log($"{collision.collider.gameObject.name}");
    //     if (collision.gameObject.TryGetComponent(out ICutable cutable))
    //     {
    //         cutable.DestructFromCut();
    //         _hotWheelAudioHandler.TryPlayCut();
    //     }
    // }
}