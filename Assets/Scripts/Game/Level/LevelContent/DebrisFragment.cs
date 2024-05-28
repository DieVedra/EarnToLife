using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DebrisFragment : MonoBehaviour
{
    private readonly float _defaultSize = 0.5f;
    private CompositeDisposable _compositeDisposable;
    private CancellationTokenSource _cancellationTokenSource;
    private Transform _fragmentTransform;
    private Collider2D _fragmentCollider2D;
    private Rigidbody2D _rigidbody2D;
    public Transform FragmentTransform => _fragmentTransform;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;

    public TypeCollider TypeCollider { get; private set;}
    public float SizeFragment { get; private set; }

    public void Init()
    {
        _fragmentTransform = transform;
        _compositeDisposable = new CompositeDisposable();
        _cancellationTokenSource = new CancellationTokenSource();
        TryCalculateSizeFragmentAndSetCollider();
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
            _rigidbody2D.AddForce(force);
        }
    }
    public void SubscribeFragment(Action operation, int layer, float delayChangeLayer)
    {
        _fragmentCollider2D.OnCollisionEnter2DAsObservable().Subscribe(_ =>
        {
            operation?.Invoke();
            SetLayerFragments(layer, delayChangeLayer).Forget();
        }).AddTo(_compositeDisposable);
    }
    private async UniTaskVoid SetLayerFragments(int layer, float delayChangeLayer)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayChangeLayer), cancellationToken: _cancellationTokenSource.Token);
        _compositeDisposable.Clear();
        _fragmentTransform.gameObject.layer = layer;
    }

    private void TryCalculateSizeFragmentAndSetCollider()
    {
        if (_fragmentTransform.TryGetComponent(out PolygonCollider2D polygonCollider2D))
        {
            TypeCollider = TypeCollider.Polygon;
            CalculateSizePolygonCollider(polygonCollider2D);
            SetCollider(polygonCollider2D);
        }
        else if(_fragmentTransform.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            TypeCollider = TypeCollider.Box;
            CalculateSizeBoxCollider(boxCollider2D);
            SetCollider(boxCollider2D);
        }
        else if(_fragmentTransform.TryGetComponent(out CapsuleCollider2D capsuleCollider2D))
        {
            TypeCollider = TypeCollider.Capsule;
            CalculateSizeCapsuleCollider(capsuleCollider2D);
            SetCollider(capsuleCollider2D);
        }
        else if(_fragmentTransform.TryGetComponent(out CircleCollider2D circleCollider2D))
        {
            TypeCollider = TypeCollider.Circle;
            CalculateSizeCircleCollider(circleCollider2D);
            SetCollider(circleCollider2D);
        }
        else
        {
            TypeCollider = TypeCollider.Other;
            SizeFragment = _defaultSize;
            SetCollider();
        }
    }

    private void CalculateSizePolygonCollider(PolygonCollider2D polygonCollider2D)
    {
        Vector2[] pathPoints = polygonCollider2D.GetPath(0);
        float maxY = pathPoints.OrderByDescending(p => p.y).FirstOrDefault().y;
        float maxX = pathPoints.OrderByDescending(p => p.x).FirstOrDefault().x;

        float minY = pathPoints.OrderBy(p => p.y).FirstOrDefault().y;
        float minX = pathPoints.OrderBy(p => p.x).FirstOrDefault().x;
        SizeFragment = Vector2.Distance(new Vector2(maxX, maxY), new Vector2(minX, minY));
    }
    private void CalculateSizeBoxCollider(BoxCollider2D boxCollider2D)
    {
        SizeFragment = Mathf.Sqrt(boxCollider2D.size.x * boxCollider2D.size.x + boxCollider2D.size.y * boxCollider2D.size.y);
    }
    private void CalculateSizeCapsuleCollider(CapsuleCollider2D capsuleCollider2D)
    {
        float height = capsuleCollider2D.size.y - capsuleCollider2D.size.x;
        SizeFragment = Mathf.Sqrt(capsuleCollider2D.size.x * capsuleCollider2D.size.x + height * height);
    }
    private void CalculateSizeCircleCollider(CircleCollider2D circleCollider2D)
    {
        SizeFragment = circleCollider2D.radius;
    }

    private void SetCollider(Collider2D collider2D = null)
    {
        if (collider2D != null)
        {
            _fragmentCollider2D = collider2D;
        }
        else
        {
            _fragmentCollider2D = _fragmentTransform.GetComponent<Collider2D>();
        }
    }
}