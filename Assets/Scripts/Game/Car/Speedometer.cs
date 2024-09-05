using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Speedometer
{
    private readonly float _speedMultiplier = 2.5f;
    private readonly IMovementForward _movementForward;
    private Rigidbody2D _rigidbody;
    public ReactiveProperty<float> CurrentSpeedReactiveProperty = new ReactiveProperty<float>();
    public int CurrentSpeedInt { get; private set; } = 0;
    public float CurrentSpeedFloat => CurrentSpeedReactiveProperty.Value;
    public bool IsMovementForward => _movementForward.IsMovementForward;

    public Speedometer(IMovementForward transmission, Rigidbody2D rigidbody2D)
    {
        _movementForward = transmission;
        _rigidbody = rigidbody2D;
    }
    public void Update()
    {
        CurrentSpeedReactiveProperty.Value = _rigidbody.velocity.magnitude * _speedMultiplier;
        CurrentSpeedInt = Mathf.RoundToInt(CurrentSpeedReactiveProperty.Value);
    }
}
