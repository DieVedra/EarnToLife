using System;
using UnityEngine;

public class FrontDoorDestructionHandler : DestructionHandler
{
    public readonly int StrengthDoor;
    private readonly DoorRef _doorRef;
    private readonly DestructionEffectsHandler _destructionEffectsHandler;
    private readonly Transform _doorNormal;
    private readonly Transform _doorDamaged1;
    private readonly Transform _doorDamaged2;
    private readonly bool _isArmored;
    private Transform _activePart;
    private Transform _rearviewMirror;
    private Rigidbody2D _rigidBody2dRearviewMirror;
    private bool _isBroken = false;
    public FrontDoorDestructionHandler(DoorRef doorRef, DestructionHandlerContent destructionHandlerContent, DestructionEffectsHandler destructionEffectsHandler,
        bool isArmored = false)
        : base(doorRef, destructionHandlerContent, " FrontDoor ")
    {
        _doorRef = doorRef;
        _destructionEffectsHandler = destructionEffectsHandler;
        _doorNormal = doorRef.DoorNormal;
        _doorDamaged1 = doorRef.DoorDamaged1;
        _doorDamaged2 = doorRef.DoorDamaged2;
        StrengthDoor = doorRef.StrengthDoors;
        if (isArmored == false)
        {
            _rearviewMirror = doorRef.RearviewMirror;
            _rigidBody2dRearviewMirror = _rearviewMirror.GetComponent<Rigidbody2D>();
            _rigidBody2dRearviewMirror.simulated = false;
        }

        _activePart = _doorNormal;
        _isArmored = isArmored;
    }
    
    public void TryDestructionMode1()
    {
        if (_isBroken == false)
        {
            _doorNormal.gameObject.SetActive(false);
            _doorDamaged1.gameObject.SetActive(true);
            _activePart = _doorDamaged1;
            if (_isArmored == false)
            {
                _rigidBody2dRearviewMirror.simulated = true;
                SetParentDebris(_rearviewMirror);
                SetCarDebrisLayerNonInteractableWithCar(_rearviewMirror);
                _destructionEffectsHandler.HitBrokenEffect(_doorRef.PointEffect.position, ImpulseNormalValue);
            }
            else
            {
                _destructionEffectsHandler.GlassBrokenEffect(_doorRef.PointEffect, ImpulseNormalValue);
            }
        }
    }
    public void TryDestructionMode2()
    {
        if (_isBroken == false)
        {
            _doorDamaged1.gameObject.SetActive(false);
            _doorDamaged2.gameObject.SetActive(true);
            _activePart = _doorDamaged2;
        }
    }

    public void TryThrowDoor()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            _activePart.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_doorRef.transform);
            SetCarDebrisLayerNonInteractableWithCar(_doorRef.transform);
        }
    }
}