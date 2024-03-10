using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BottomDestructionHandler : DestructionHandler
{
    private readonly float _halfStrength;
    private readonly BottomRef _bottomRef;
    private readonly Collider2D _collider2DStandart;
    private readonly Transform _armoredBottom;
    private readonly Transform _exhaust;
    private readonly Transform _backCar;
    private readonly BackCarDestructionHandler _backCarDestructionHandler;
    private readonly RoofDestructionHandler _roofDestructionHandler;
    private readonly SafetyFrameworkDestructionHandler _safetyFrameworkDestructionHandler;
    private readonly FrontDoorDestructionHandler _frontDoorDestructionHandler;
    private readonly BackDoorDestructionHandler _backDoorDestructionHandler;
    private readonly bool _isArmored;
    private bool _exhaustBroken = false;
    public BottomDestructionHandler(BottomRef bottomRef, BackCarDestructionHandler backCarDestructionHandler, RoofDestructionHandler roofDestructionHandler, 
        FrontDoorDestructionHandler frontDoorDestructionHandler, BackDoorDestructionHandler backDoorDestructionHandler,
        DestructionHandlerContent destructionHandlerContent, int strength, bool isArmored)
        : base(bottomRef, destructionHandlerContent, strength)
    {
        _bottomRef = bottomRef;
        _halfStrength = bottomRef.StrengthBottom * HalfStrengthMultiplier;
        _exhaust = bottomRef.Exhaust;
        _backCar = bottomRef.BackCar;
        _backCarDestructionHandler = backCarDestructionHandler;
        _roofDestructionHandler = roofDestructionHandler;
        _frontDoorDestructionHandler = frontDoorDestructionHandler;
        _backDoorDestructionHandler = backDoorDestructionHandler;
        _collider2DStandart = _bottomRef.GetComponent<Collider2D>();
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _collider2DStandart.enabled = false;
            SubscribeCollider(_bottomRef.ArmoredBottom.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
        }
        else
        {
            SubscribeCollider(_collider2DStandart, CheckCollision, TryDestruct);
        }
    }

    protected override void TryDestruct()
    {
        ApplyDamage();
        if (MaxStrength <= StrengthForDestruct)
        {
            DestructionMode2();
        }
        else if (MaxStrength <= _halfStrength)
        {
            DestructionMode1();
        }
        
    }

    private void DestructionMode1()
    {
        // CompositeDisposable.Clear();
        _frontDoorDestructionHandler.TryThrowDoor();
        _backDoorDestructionHandler.TryThrowDoor();
        TryThrowExhaust();


    }
    private void DestructionMode2()
    {
        CompositeDisposable.Clear();
        if (_isArmored == true)
        {
            ThrowArmor();
        }
        else
        {
            _collider2DStandart.enabled = false;
        }
        _frontDoorDestructionHandler.TryThrowDoor();
        _backDoorDestructionHandler.TryThrowDoor();
        TryThrowExhaust();
        _roofDestructionHandler?.DestructNow();
        _backCarDestructionHandler?.DestructNow();
        _backCar.gameObject.AddComponent<Rigidbody2D>();
        SetParentDebris(_backCar);
    }

    private void TryThrowExhaust()
    {
        if (_exhaustBroken == false)
        {
            _exhaustBroken = true;
            _exhaust.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_exhaust);
        }
    }

    private void ThrowArmor()
    {
        _armoredBottom.gameObject.AddComponent<Rigidbody2D>();
        SetParentDebris(_armoredBottom);
    }

}