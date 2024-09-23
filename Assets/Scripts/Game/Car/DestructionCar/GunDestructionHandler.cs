using System;
using UnityEngine;

public sealed class GunDestructionHandler : DestructionHandler, IDispose
{
    private readonly CarGun _carGun;
    private readonly CoupAnalyzer _coupAnalyzer;
    private readonly DestructionEffectsHandler _destructionEffectsHandler;
    private readonly CarMass _carMass;
    private readonly int _fallingContentLayer;
    private readonly Transform[] _gunParts;
    private bool _isBroken = false;
    private bool _carHasBeenCoup;
    private bool _isСrushed;

    public GunDestructionHandler(GunRef gunRefs, CarGun carGun, CarMass carMass, CoupAnalyzer coupAnalyzer,
        DestructionHandlerContent destructionHandlerContent, DestructionEffectsHandler destructionEffectsHandler, int fallingContentLayer)
    :base(gunRefs, destructionHandlerContent, maxStrength: gunRefs.StrengthGun)
    {
        _gunParts = gunRefs.GunParts;
        _carGun = carGun;
        _carMass = carMass;
        _coupAnalyzer = coupAnalyzer;
        _destructionEffectsHandler = destructionEffectsHandler;
        _fallingContentLayer = fallingContentLayer;
        SubscribeCollider(_gunParts[0].GetComponent<Collider2D>(), CollisionHandlingGun, TrySwitchMode);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
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
            SetCarDebrisLayerInteractableWithCar();
        }
    }

    private bool CollisionHandlingGun(Collision2D collision)
    {
        bool result = false;
        if (1 << _fallingContentLayer == 1 << collision.gameObject.layer && _isСrushed == false)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                SetImpulseNormalAndHitPosition(collision);
                ImpulseNormalValue += rigidbody2D.mass;
                if (rigidbody2D.mass > MaxStrength)
                {
                    _isСrushed = true;
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        if (_coupAnalyzer.CarIsCoup == true || _coupAnalyzer.CarIsTilted == true)
        {
            if ((base.CheckCollision(collision) == true)
                && _carHasBeenCoup == false)
            {
                _carHasBeenCoup = true;
                SetImpulseNormalAndHitPosition(collision);
                ImpulseNormalValue += _carMass.Mass;
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            if (base.CheckCollisionAndMinSpeed(collision) == true)
            {
                SetImpulseNormalAndHitPosition(collision);
                result = true;
            }
            else
            {
                SetImpulseNormalAndHitPosition(collision);
                PlaySoftHitSound();
                result = false;
            }
        }
        return result;
    }
    protected override void TrySwitchMode()
    {
        if (ImpulseNormalValue > MaxStrength)
        {
            PlayEffect();
            TryDestruct();
        }
        else
        {
            RecalculateStrength();
            PlaySoftHitSound();
        }
    }

    private void PlayEffect()
    {
        _destructionEffectsHandler.HitBrokenEffect(HitPosition, ImpulseNormalValue);
    }
}