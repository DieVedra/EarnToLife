using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DebrisFragment
{
    private readonly CompositeDisposable _compositeDisposable;
    private readonly Transform _fragmentTransform;
    private readonly Collider2D _fragmentCollider2D;
    private Rigidbody2D _rigidbody2D;

    public Transform FragmentTransform => _fragmentTransform;

    public DebrisFragment(Transform fragmentTransform)
    {
        _fragmentTransform = fragmentTransform;
        _fragmentCollider2D = fragmentTransform.GetComponent<Collider2D>();
        _compositeDisposable = new CompositeDisposable();
    }

    public void Dispose()
    {
        _compositeDisposable.Clear();
    }
    public void InitRigidBody()
    {
        _rigidbody2D = _fragmentTransform.gameObject.AddComponent<Rigidbody2D>(); 
    }

    public void TryAddForce(Vector2 force)
    {
        if (_rigidbody2D != null)
        {
            _rigidbody2D.AddForce(force);
        }
    }
    public void SubscribeFragment(Action operation, int layer, float delayChangeLayer)
    {
        _fragmentCollider2D.OnCollisionEnter2DAsObservable().Subscribe(_ =>
        {
            operation?.Invoke();
            SetLayerFragments(layer, delayChangeLayer);
        }).AddTo(_compositeDisposable);
    }
    private async void SetLayerFragments(int layer, float delayChangeLayer)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayChangeLayer));
        _compositeDisposable.Clear();
        _fragmentTransform.gameObject.layer = layer;
    }
    
}