using UnityEngine;

public sealed class GunDestructionHandler : DestructionHandler
{
    private Transform _gunTransform;
    private Collider2D _gunCollider2D;
    private CarGun _carGun;
    private Transform[] _gunParts;
    
    public GunDestructionHandler(GunRef gunRefs, CarGun carGun, DestructionHandlerContent destructionHandlerContent)
    :base(gunRefs, destructionHandlerContent, gunRefs.StrengthGun)
    {
        _gunTransform = gunRefs.transform;
        _gunParts = gunRefs.GunParts;
        _carGun = carGun;
        _gunCollider2D = _gunParts[0].GetComponent<Collider2D>();
        SubscribeCollider(_gunCollider2D, CheckCollision, TryDestruct);
    }
    protected override void TryDestruct()
    {
        ApplyDamage();
        if (MaxStrength <= StrengthForDestruct)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        CompositeDisposable.Clear();
        _carGun.GunDisableFromDestruct();
        _gunTransform.SetParent(DebrisParent);
        SetCarDebrisLayer();
        for (int i = 0; i < _gunParts.Length; i++)
        {
            _gunParts[i].gameObject.AddComponent<Rigidbody2D>();
        }
    }
}