using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GroundAnalyzer
{
    private readonly int _asphaltLayer;
    private readonly int _groundLayer;
    private readonly float _distance = 0.1f;
    private readonly float _delay = 0.12f;
    private readonly CarWheel _frontWheel;
    private readonly CarWheel _backWheel;
    private readonly LayerMask _contactMask;
    private bool _carBrokenIntoTwoParts = false;
    private RaycastHit2D _frontWheelHit;
    private RaycastHit2D _backWheelHit;
    private float _time = 0f;
    public Vector2 FrontWheelPointContact { get; private set; }
    public Vector2 BackWheelPointContact { get; private set; }

    public ReactiveProperty<bool> FrontWheelOnGroundReactiveProperty = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelOnGroundReactiveProperty = new ReactiveProperty<bool>();
    
    public ReactiveProperty<bool> FrontWheelOnAsphaltReactiveProperty = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelOnAsphaltReactiveProperty = new ReactiveProperty<bool>();
    public bool FrontWheelContact { get; private set; } = false;
    public bool BackWheelContact { get; private set; } = false;
    public GroundAnalyzer(CarWheel frontWheel, CarWheel backWheel, ReactiveCommand onCarBrokenIntoTwoParts,
        LayerMask contactMask, int asphaltLayer, int groundLayer)
    {
        _frontWheel = frontWheel;
        _backWheel = backWheel;
        _contactMask = contactMask;
        _asphaltLayer = asphaltLayer;
        _groundLayer = groundLayer;
        onCarBrokenIntoTwoParts.Subscribe(_ => { CarBrokenIntoTwoParts();});
    }

    public void Update()
    {
        if (_time <= 0f)
        {
            _time = _delay;
            CheckCircleFrontWheel();
            if (_carBrokenIntoTwoParts == false)
            {
                CheckCircleBackWheel();
            }
        }
        else
        {
            _time -= Time.deltaTime;
        }
    }
    private void CheckCircleFrontWheel()
    {
        _frontWheelHit = Physics2D.CircleCast(_frontWheel.Position, _frontWheel.Radius,
            Vector2.down, _distance, _contactMask.value);
        if (_frontWheelHit != null)
        {
            CheckLayer(FrontWheelOnGroundReactiveProperty, FrontWheelOnAsphaltReactiveProperty, _frontWheelHit,
            SetContactPointFrontWheelAndSetKeyTrue,SetKeyFalseFrontWheelContact);
        }
    }
    private void CheckCircleBackWheel()
    {
        _backWheelHit = Physics2D.CircleCast(_backWheel.Position, _backWheel.Radius,
            Vector2.down, _distance, _contactMask.value);
        if (_backWheelHit != null)
        {
            CheckLayer(BackWheelOnGroundReactiveProperty, BackWheelOnAsphaltReactiveProperty, _backWheelHit,
                SetContactPointBackWheelAndSetKeyTrue, SetKeyFalseBackWheelContact);
        }
    }

    private void CheckLayer(ReactiveProperty<bool> wheelOnGroundReactiveProperty,
        ReactiveProperty<bool> wheelOnAsphaltReactiveProperty, RaycastHit2D hit,
        Action<RaycastHit2D> setContactPointWheel, Action setKeyFalseContact)
    {
        if (hit.transform.gameObject.layer == _groundLayer)
        {
            wheelOnGroundReactiveProperty.Value = true;
            wheelOnAsphaltReactiveProperty.Value = false;
            setContactPointWheel(hit);
        }
        else if (hit.transform.gameObject.layer == _asphaltLayer)
        {
            wheelOnGroundReactiveProperty.Value = false;
            wheelOnAsphaltReactiveProperty.Value = true;
            setContactPointWheel(hit);
        }
        else
        {
            wheelOnGroundReactiveProperty.Value = false;
            wheelOnAsphaltReactiveProperty.Value = false;
            setKeyFalseContact?.Invoke();
        }
    }
    private void SetContactPointFrontWheelAndSetKeyTrue(RaycastHit2D wheelHit)
    {
        FrontWheelContact = true;
        FrontWheelPointContact = wheelHit.point;
    }
    private void SetContactPointBackWheelAndSetKeyTrue(RaycastHit2D wheelHit)
    {
        BackWheelContact = true;
        BackWheelPointContact = wheelHit.point;
    }
    private void SetKeyFalseFrontWheelContact()
    {
        FrontWheelContact = false;
    }
    private void SetKeyFalseBackWheelContact()
    {
        BackWheelContact = false;
    }
    private void CarBrokenIntoTwoParts()
    {
        _carBrokenIntoTwoParts = true;
    }
}