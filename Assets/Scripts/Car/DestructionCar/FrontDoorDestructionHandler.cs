using System;
using UnityEngine;

public class FrontDoorDestructionHandler : DestructionHandler
{
    public readonly int StrengthDoor;
    private readonly DoorRef _doorRef;
    private readonly Action<Vector2, float> _effect;
    private readonly Transform _doorNormal;
    private readonly Transform _doorDamaged1;
    private readonly Transform _doorDamaged2;
    private readonly bool _isArmored;
    private Transform _activePart;
    private Transform _rearviewMirror;
    private bool _isBroken = false;
    public FrontDoorDestructionHandler(DoorRef doorRef, DestructionHandlerContent destructionHandlerContent, Action<Vector2, float> effect,
        bool isArmored = false)
        : base(doorRef, destructionHandlerContent)
    {
        _doorRef = doorRef;
        _effect = effect;
        _doorNormal = doorRef.DoorNormal;
        _doorDamaged1 = doorRef.DoorDamaged1;
        _doorDamaged2 = doorRef.DoorDamaged2;
        StrengthDoor = doorRef.StrengthDoors;
        _rearviewMirror = doorRef.RearviewMirror;
        _activePart = _doorNormal;
        _isArmored = isArmored;
    }
    
    public void TryDestructionMode1()
    {
        if (_isBroken == false)
        {
            PlayEffect();
            _doorNormal.gameObject.SetActive(false);
            _doorDamaged1.gameObject.SetActive(true);
            _activePart = _doorDamaged1;
            if (_isArmored == false)
            {
                _rearviewMirror.gameObject.AddComponent<Rigidbody2D>();
                SetParentDebris(_rearviewMirror);
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
    public void DestructionMode3()
    {
        CompositeDisposable.Clear();
    }

    public void TryThrowDoor()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            _activePart.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_doorRef.transform);
        }
    }

    private void PlayEffect()
    {
        _effect.Invoke(_doorRef.PointEffect.position, ImpulseNormalValue);
    }
}