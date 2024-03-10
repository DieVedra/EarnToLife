using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class HotWheel
{
    public readonly HotWheelRef HotWheelRef;
    private readonly Transform _wheel1;
    private readonly Transform _wheel2;
    private readonly float _hotWheelRotationSpeed;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private ICutable _cutable;
    public HotWheel(HotWheelRef hotWheelRef, float hotWheelRotationSpeed)
    {
        HotWheelRef = hotWheelRef;
        _hotWheelRotationSpeed = hotWheelRotationSpeed;
        _wheel1 = hotWheelRef.Wheel1;
        _wheel2 = hotWheelRef.Wheel2;
        SubscribeUpdate();
        SubscribeTriggers();
    }
    public void Dispose()
    {
        _compositeDisposable.Clear();
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
        }).AddTo(_compositeDisposable);
    }
    private void SubscribeTriggers()
    {
        _wheel1.GetComponent<Collider2D>().OnCollisionEnter2DAsObservable()
            .Where(CheckCollision)
            .Subscribe(_ =>
            {
                _cutable.DestructFromCut();
            }).AddTo(_compositeDisposable);
        
        _wheel2.GetComponent<Collider2D>().OnCollisionEnter2DAsObservable()
            .Where(CheckCollision)
            .Subscribe(_ =>
            {
                _cutable.DestructFromCut();
            }).AddTo(_compositeDisposable);
    }
    private bool CheckCollision(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICutable cutable))
        {
            _cutable = cutable;
            return true;
        }
        else
        {
            return false;
        }
    }
}