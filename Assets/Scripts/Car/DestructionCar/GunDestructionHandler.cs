using UnityEngine;

public sealed class GunDestructionHandler : DestructionHandler, IDispose
{
    private readonly CarGun _carGun;
    private readonly Transform[] _gunParts;
    private bool _isBroken = false;
    public GunDestructionHandler(GunRef gunRefs, CarGun carGun, DestructionHandlerContent destructionHandlerContent)
    :base(gunRefs, destructionHandlerContent, gunRefs.StrengthGun)
    {
        _gunParts = gunRefs.GunParts;
        _carGun = carGun;
        SubscribeCollider(_gunParts[0].GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
    protected override void TrySwitchMode()
    {
        if (ValueNormalImpulse > MaxStrength)
        {
            TryDestruct();
        }
        else
        {
            RecalculateStrength();
        }
    }
    public void TryDestruct()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            Dispose();
            _carGun.GunDisableFromDestruct();
            for (int i = 0; i < _gunParts.Length; i++)
            {
                _gunParts[i].gameObject.AddComponent<Rigidbody2D>();
                SetParentDebris(_gunParts[i]);
            }
            SetParentDebris();
            SetCarDebrisLayer();
        }
    }
}