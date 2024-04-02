using System;
using UnityEngine;

public class BackDoorDestructionHandler : DestructionHandler
{
    private readonly DoorRef _doorRef;
    private readonly Action<Vector2, float> _effect;
    public readonly int StrengthDoor;
    private readonly Transform _doorNormal;
    private readonly Transform _doorDamaged1;
    private readonly Transform _doorDamaged2;
    private readonly Transform[] _parts;
    private bool _isBroken = false;

    public BackDoorDestructionHandler(DoorRef doorRef, DestructionHandlerContent destructionHandlerContent, Action<Vector2, float> effect) 
        : base(doorRef, destructionHandlerContent, maxStrength: doorRef.StrengthDoors)
    {
        _doorRef = doorRef;
        _effect = effect;
        _doorNormal = doorRef.DoorNormal;
        _doorDamaged1 = doorRef.DoorDamaged1;
        _doorDamaged2 = doorRef.DoorDamaged2;
        StrengthDoor = doorRef.StrengthDoors;
        _parts = new[] {_doorNormal, _doorDamaged1, _doorDamaged2};
    }
    public void DestructionMode1()
    {
        if (_isBroken == false)
        {
            PlayEffect();
            _doorNormal.gameObject.SetActive(false);
            _doorDamaged1.gameObject.SetActive(true);
            DestructionMode = DestructionMode.Mode1;
        }
    }
    public void TryDestructionMode2()
    {
        if (_isBroken == false)
        {
            if (DestructionMode == DestructionMode.ModeDefault)
            {
                DestructionMode1();
            }
            _doorDamaged1.gameObject.SetActive(false);
            _doorDamaged2.gameObject.SetActive(true);
            DestructionMode = DestructionMode.Mode2;
        }
    }
    public void TryThrowDoor()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            SetParentDebris();
            SetCarDebrisLayerNonInteractableWithCar();
            for (int i = 0; i < _parts.Length; i++)
            {
                if (_parts[i].gameObject.activeSelf == true)
                {
                    TryAddRigidBody(_parts[i].gameObject);
                }
            }
        }
    }

    private void PlayEffect()
    {
        _effect.Invoke(_doorRef.PointEffect.position, ImpulseNormalValue);
    }
}