using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Trap : MonoBehaviour
{
    [SerializeField] private Collider2D _trigger;
    private Rigidbody2D _rigidbody2D;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();

private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = true;
        _trigger.OnTriggerEnter2DAsObservable().Subscribe(_trigger =>
        {
            _rigidbody2D.isKinematic = false;
            _compositeDisposable.Clear();
        }).AddTo(_compositeDisposable);
    }
}
