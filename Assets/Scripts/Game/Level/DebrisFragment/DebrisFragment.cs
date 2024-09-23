using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DebrisFragment : MonoBehaviour
{
    private readonly float _speedMultiplier = 2.5f;
    private readonly float _forceMinValue = 0f;
    private readonly float _forceMaxValue = 5f;
    private readonly float _defaultVolume = 0.9f;
    private int _layerDebris, _layerCar;
    private readonly float _delayChangeLayer = 0.3f;
    private CompositeDisposable _compositeDisposable;
    private CancellationTokenSource _cancellationTokenSource;
    private Transform _fragmentTransform;
    private Collider2D _fragmentCollider2D;
    private Rigidbody2D _rigidbody2D;
    private DebrisFragmentCalculate _debrisFragmentCalculate;
    private float _forceHit;
    private Action<float> _debrisHitSound;

    public Transform FragmentTransform => _fragmentTransform;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;

    public TypeCollider TypeCollider { get; private set;}
    public float SizeFragment { get; private set; }

    public void Init(Action<float> debrisHitSound, int layerDebris, int layerCar)
    {
        _layerDebris = layerDebris;
        _layerCar = layerCar;
        _debrisHitSound = debrisHitSound;
        _fragmentTransform = transform;
        _compositeDisposable = new CompositeDisposable();
        _debrisFragmentCalculate = new DebrisFragmentCalculate(_fragmentTransform);
        TypeCollider = _debrisFragmentCalculate.GetSizeFragmentAndSetCollider();
        SizeFragment = _debrisFragmentCalculate.SizeFragment;
        _fragmentCollider2D = _debrisFragmentCalculate.FragmentCollider2D;
        _cancellationTokenSource = new CancellationTokenSource();
    }
    public void Dispose()
    {
        _compositeDisposable.Clear();
        _cancellationTokenSource.Cancel();
    }
    public void InitRigidBody()
    {
        if (_fragmentTransform.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            _rigidbody2D = rigidbody2D;
        }
        else
        {
            _rigidbody2D = _fragmentTransform.gameObject.AddComponent<Rigidbody2D>();
        }
    }

    public void TryAddForce(Vector2 force)
    {
        if (_rigidbody2D != null)
        {
            _rigidbody2D.AddForce(force, ForceMode2D.Force);
        }
    }
    public void SubscribeFragment()
    {
        _fragmentCollider2D.OnCollisionEnter2DAsObservable().Subscribe(collision =>
        {
            OnCollisionCollider(collision);
        }).AddTo(_compositeDisposable);
            _fragmentCollider2D.OnTriggerEnter2DAsObservable().Subscribe(collider =>
            {
                OnCollisionTrigger(collider);
        }).AddTo(_compositeDisposable);
    }

    private void OnCollisionCollider(Collision2D collision2D)
    {
        OnCollision(collision2D.collider, GetVolumeValueFromForceHit(collision2D.contacts));
    }

    private void OnCollisionTrigger(Collider2D collider2D)
    {
        OnCollision(collider2D, _defaultVolume);
    }

    private void OnCollision(Collider2D collider2D, float volume)
    {
        _debrisHitSound?.Invoke(volume);
        if (collider2D.transform.parent.TryGetComponent(out Zombie zombie))
        {
            zombie.TryBreakOnImpact(_rigidbody2D.velocity.magnitude * _speedMultiplier);
            SetLayerDebris();
        }
        else if (collider2D.transform.parent.gameObject.layer == _layerCar)
        {
            LayerFragments().Forget();
        }
        else
        {
            SetLayerDebris();
        }
    }
    private async UniTaskVoid LayerFragments()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayChangeLayer), cancellationToken: _cancellationTokenSource.Token);
        SetLayerDebris();
        UnsubscribeCollider();
    }
    private void SetLayerDebris()
    {
        _fragmentTransform.gameObject.layer = _layerDebris;
    }

    private void UnsubscribeCollider()
    {
        _compositeDisposable.Clear();
    }
    private float GetVolumeValueFromForceHit(ContactPoint2D[] contactPoints)
    {
        _forceHit = 0f;

        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (_forceHit < contactPoints[i].normalImpulse)
            {
                _forceHit = contactPoints[i].normalImpulse;
            }
        }
        
        return Mathf.InverseLerp(_forceMinValue, _forceMaxValue, _forceHit);
    }
}