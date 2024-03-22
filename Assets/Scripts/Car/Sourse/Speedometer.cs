using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Speedometer
{
    private readonly float _speedMultiplier = 2.5f;
    private Rigidbody2D _rigidbody;
    public ReactiveProperty<float> CurrentSpeedReactiveProperty = new ReactiveProperty<float>();
    public int CurrentSpeedInt { get; private set; } = 0;
    public float CurrentSpeedFloat => CurrentSpeedReactiveProperty.Value;
    public Speedometer(Rigidbody2D rigidbody2D)
    {
        _rigidbody = rigidbody2D;
    }
    public void Update()
    {
        CurrentSpeedReactiveProperty.Value = _rigidbody.velocity.magnitude * _speedMultiplier;
        CurrentSpeedInt = Mathf.RoundToInt(CurrentSpeedReactiveProperty.Value);
    }
}
