using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BottomDestructionHandler : DestructionHandler, IDispose
{
    private readonly BottomRef _bottomRef;
    private readonly Collider2D _collider2DStandart;
    private readonly Transform _armoredBottom;
    private readonly Transform _exhaust;
    private readonly Transform _backCar;
    private readonly BackCarHandler _backCarHandler;
    private readonly RoofDestructionHandler _roofDestructionHandler;
    private readonly SafetyFrameworkDestructionHandler _safetyFrameworkDestructionHandler;
    private readonly FrontDoorDestructionHandler _frontDoorDestructionHandler;
    private readonly BackDoorDestructionHandler _backDoorDestructionHandler;
    private readonly bool _isArmored;
    private bool _exhaustBroken = false;
    private DestructionMode _destructionMode = DestructionMode.ModeDefault;
    public BottomDestructionHandler(BottomRef bottomRef, BackCarHandler backCarHandler, RoofDestructionHandler roofDestructionHandler, 
        FrontDoorDestructionHandler frontDoorDestructionHandler, BackDoorDestructionHandler backDoorDestructionHandler,
        DestructionHandlerContent destructionHandlerContent, int strength, bool isArmored)
        : base(bottomRef, destructionHandlerContent, strength)
    {
        _bottomRef = bottomRef;
        _exhaust = bottomRef.Exhaust;
        _backCar = bottomRef.BackCar;
        _backCarHandler = backCarHandler;
        _roofDestructionHandler = roofDestructionHandler;
        _frontDoorDestructionHandler = frontDoorDestructionHandler;
        _backDoorDestructionHandler = backDoorDestructionHandler;
        _collider2DStandart = _bottomRef.GetComponent<Collider2D>();
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _collider2DStandart.enabled = false;
            SubscribeCollider(_bottomRef.ArmoredBottom.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
        }
        else
        {
            SubscribeCollider(_collider2DStandart, CheckCollision, TrySwitchMode);
        }
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }

    protected override void TrySwitchMode()
    {
        Debug.Log("BottomDestruction TrySwitchMode");
        if (ValueNormalImpulse > MaxStrength)
        {
            DestructionMode2();
            RecalculateStrength();
        }
        else if (ValueNormalImpulse > HalfStrength)
        {
            DestructionMode1();
            RecalculateStrength();
        }else if (ValueNormalImpulse > MinStrength)
        {
            RecalculateStrength();
        }
    }

    private void DestructionMode1()
    {
        _frontDoorDestructionHandler.TryThrowDoor();
        _backDoorDestructionHandler.TryThrowDoor();
        TryThrowExhaust();
        _destructionMode = DestructionMode.Mode1;
        Debug.Log("           DestructionMode1");

    }
    private void DestructionMode2()
    {
        if (_destructionMode != DestructionMode.Mode1)
        {
            DestructionMode1();
        }
        CompositeDisposable.Clear();
        if (_isArmored == true)
        {
            ThrowArmor();
        }
        else
        {
            _collider2DStandart.enabled = false;
        }
        _roofDestructionHandler?.DestructNow();
        _backCarHandler?.DestructNow();
        
        
        
        SetParentDebris(_backCar);
        _destructionMode = DestructionMode.Mode2;
        Debug.Log("    DestructionMode2");

    }

    private void TryThrowExhaust()
    {
        if (_exhaustBroken == false)
        {
            _exhaustBroken = true;
            TryAddRigidBody(_exhaust.gameObject);
            SetParentDebris(_exhaust);
        }
    }
    private void ThrowArmor()
    {
        TryAddRigidBody(_armoredBottom.gameObject);
        SetParentDebris(_armoredBottom);
    }
}