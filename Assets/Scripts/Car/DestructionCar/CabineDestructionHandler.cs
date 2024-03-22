using UnityEngine;

public class CabineDestructionHandler :DestructionHandler, IDispose
{
    private CabineRef _cabineRef;
    private Transform _headrest;
    private Collider2D _helmetCollider;
    private Collider2D _headrestCollider;
    private bool _isBroken = false;
    public CabineDestructionHandler(CabineRef cabineRef, DestructionHandlerContent destructionHandlerContent)
        : base(cabineRef, destructionHandlerContent)
    {
        _cabineRef = cabineRef;
        _headrest = cabineRef.Headrest;
        _helmetCollider = cabineRef.Helmet.GetComponent<Collider2D>();
        _headrestCollider = cabineRef.Headrest.GetComponent<Collider2D>();
        SubscribeCollider(_helmetCollider, CheckCollision, TryDriverDestruction);
        SubscribeCollider(_headrestCollider, CheckCollision, TryDriverDestruction);
    }

    public void TryDriverDestruction()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            CompositeDisposable.Clear();
            TryAddRigidBody(_headrest.gameObject);
            SetParentDebris(_headrest);
            Debug.Log("game over driver crushed");
        }
    }
    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
}