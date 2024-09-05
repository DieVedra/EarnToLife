using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission : IMovementForward
{
    private float _multiplier = 2000f;
    private float _gearRatio;
    public float Direction { get; set; } = 0;
    public float GearRatio => _gearRatio * Direction * _multiplier;
    public bool IsMovementForward => Direction < 0f ? true : false;
    public Transmission(float gearRatio)
    {
        _gearRatio = gearRatio;
    }
}
