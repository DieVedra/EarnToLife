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
    private readonly ExhaustHandler _exhaustHandler;
    private readonly DestructionAudioHandler _destructionAudioHandler;
    private readonly bool _isArmored;
    private bool _exhaustBroken = false;
    public BottomDestructionHandler(BottomRef bottomRef, BackCarHandler backCarHandler, RoofDestructionHandler roofDestructionHandler, 
        FrontDoorDestructionHandler frontDoorDestructionHandler, BackDoorDestructionHandler backDoorDestructionHandler, ExhaustHandler exhaustHandler,
        DestructionHandlerContent destructionHandlerContent, DestructionAudioHandler destructionAudioHandler,int strength, bool isArmored)
        : base(bottomRef, destructionHandlerContent, " Bottom ", destructionAudioHandler, strength)
    {
        _bottomRef = bottomRef;
        _exhaust = bottomRef.Exhaust;
        _backCar = bottomRef.BackCar;
        _backCarHandler = backCarHandler;
        _roofDestructionHandler = roofDestructionHandler;
        _frontDoorDestructionHandler = frontDoorDestructionHandler;
        _backDoorDestructionHandler = backDoorDestructionHandler;
        _exhaustHandler = exhaustHandler;
        _destructionAudioHandler = destructionAudioHandler;
        _collider2DStandart = _bottomRef.GetComponent<Collider2D>();
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _collider2DStandart.enabled = false;
            _armoredBottom = _bottomRef.ArmoredBottom;
            SubscribeCollider(_bottomRef.ArmoredBottom.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
        else
        {
            SubscribeCollider(_collider2DStandart, CollisionHandling, TrySwitchMode);
        }
    }
    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
    protected override void TrySwitchMode()
    {
        if (ImpulseNormalValue > MaxStrength)
        {
            DestructionMode2();
            RecalculateStrength();
            _destructionAudioHandler.PlayHardHit(ImpulseNormalValue);
            Debug.Log($" bottom impulse: {ImpulseNormalValue}");
        }
        else if (ImpulseNormalValue > HalfStrength)
        {
            DestructionMode1();
            RecalculateStrength();
            _destructionAudioHandler.PlayHardHit(ImpulseNormalValue);
            Debug.Log($" bottom impulse: {ImpulseNormalValue}");

        }
        else if (ImpulseNormalValue > MinStrength)
        {
            RecalculateStrength();
            _destructionAudioHandler.PlayHardHit(ImpulseNormalValue);
            Debug.Log($" bottom impulse: {ImpulseNormalValue}");
        }
    }

    private void DestructionMode1()
    {
        _frontDoorDestructionHandler.TryThrowDoor();
        _backDoorDestructionHandler.TryThrowDoor();
        TryThrowExhaust();
        DestructionMode = DestructionMode.Mode1;
    }
    private void DestructionMode2()
    {
        if (DestructionMode != DestructionMode.Mode1)
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
        SetCarDebrisLayerInteractableWithCar(_backCar);
        DestructionMode = DestructionMode.Mode2;

    }

    private void TryThrowExhaust()
    {
        if (_exhaustBroken == false)
        {
            _exhaustBroken = true;
            _exhaustHandler.SetPoint2();
            TryAddRigidBody(_exhaust.gameObject);
            SetParentDebris(_exhaust);
            SetCarDebrisLayerNonInteractableWithCar(_exhaust);
        }
    }
    private void ThrowArmor()
    {
        TryAddRigidBody(_armoredBottom.gameObject);
        SetParentDebris(_armoredBottom);
        SetCarDebrisLayerNonInteractableWithCar(_armoredBottom);
    }
}