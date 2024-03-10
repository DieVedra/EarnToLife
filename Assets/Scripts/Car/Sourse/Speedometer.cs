using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Speedometer
{
    private readonly float _speedMultiplier = 2.5f;
    private Rigidbody2D _rigidbody;
    public int CurrentSpeedInt { get; private set; } = 0;
    public float CurrentSpeedFloat { get; private set; } = 0;
    public Speedometer(Rigidbody2D rigidbody2D)
    {
        _rigidbody = rigidbody2D;
    }
    public void Update()
    {
        CurrentSpeedFloat = _rigidbody.velocity.magnitude * _speedMultiplier;
        CurrentSpeedInt = Mathf.RoundToInt(CurrentSpeedFloat);
    }
}
