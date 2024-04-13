using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class HotWheel
{
    public readonly HotWheelRef HotWheelRef;
    private readonly float _delayDisposeSlit = 2f;
    private readonly HotWheelAudioHandler _hotWheelAudioHandler;
    private readonly Transform _wheel1;
    private readonly Transform _wheel2;
    private readonly float _hotWheelRotationSpeed;
    private CompositeDisposable _compositeDisposableRotate = new CompositeDisposable();
    private CompositeDisposable _compositeDisposableSlit = new CompositeDisposable();
    private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    private Rigidbody2D _wheel1Rigidbody2D;
    private Rigidbody2D _wheel2Rigidbody2D;

    public HotWheel(HotWheelRef hotWheelRef, HotWheelAudioHandler hotWheelAudioHandler, float hotWheelRotationSpeed)
    {
        HotWheelRef = hotWheelRef;
        _hotWheelAudioHandler = hotWheelAudioHandler;
        _hotWheelRotationSpeed = hotWheelRotationSpeed;
        _wheel1 = hotWheelRef.Wheel1;
        _wheel2 = hotWheelRef.Wheel2;
        SubscribeUpdate();
        SubscribeToSlit();
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
        _compositeDisposableRotate.Clear();
        _wheel1Rigidbody2D = HotWheelRef.Wheel1.gameObject.AddComponent<Rigidbody2D>();
        _wheel2Rigidbody2D = HotWheelRef.Wheel2.gameObject.AddComponent<Rigidbody2D>();
        _wheel1Rigidbody2D.AddTorque(100f);
        _wheel2Rigidbody2D.AddTorque(100f);
        
        
        _hotWheelAudioHandler.StopPlayRotateWheels().Forget();
        DelayDisposeSlit().Forget();
    }

    private async UniTaskVoid DelayDisposeSlit()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDisposeSlit),  cancellationToken: _cancellationToken.Token);
        _compositeDisposableSlit.Clear();
    }
    private void RotateWheels()
    {
        _wheel1.Rotate(Vector3.forward, _hotWheelRotationSpeed);
        _wheel2.Rotate(Vector3.forward, _hotWheelRotationSpeed);
    }
    private void SubscribeUpdate()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            RotateWheels();
        }).AddTo(_compositeDisposableRotate);
    }
    private void SubscribeToSlit()
    {
        _wheel1.GetComponent<Collider2D>().OnCollisionEnter2DAsObservable()
            .Do(Slit).Subscribe().AddTo(_compositeDisposableSlit);
        
        _wheel2.GetComponent<Collider2D>().OnCollisionEnter2DAsObservable()
            .Do(Slit).Subscribe().AddTo(_compositeDisposableSlit);
    }
    private void Slit(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICutable cutable))
        {
            cutable.DestructFromCut();
            _hotWheelAudioHandler.TryPlayCut().Forget();
        }
    }
}