using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class GroundAnalyzer
{
    private readonly int _asphaltLayer;
    private readonly int _groundLayer;
    private readonly int _zombieBloodLayer;
    private readonly float _distance = 0.1f;
    private readonly CarWheel _frontWheel;
    private readonly CarWheel _backWheel;
    private readonly LayerMask _contactMask;
    private bool _carBrokenIntoTwoParts = false;
    private bool _isSplittingFrame;
    private RaycastHit2D _frontWheelHit;
    private RaycastHit2D _backWheelHit;
    public Vector2 FrontWheelPointContact { get; private set; }
    public Vector2 BackWheelPointContact { get; private set; }

    public ReactiveProperty<bool> FrontWheelOnGroundReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelOnGroundReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    
    public ReactiveProperty<bool> FrontWheelOnAsphaltReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelOnAsphaltReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    
    public ReactiveProperty<bool> FrontWheelOnZombieReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelOnZombieReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    
    public ReactiveProperty<bool> FrontWheelContactReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelContactReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    public bool FrontWheelContact => FrontWheelContactReactiveProperty.Value;
    public bool BackWheelContact => BackWheelContactReactiveProperty.Value;
    public GroundAnalyzer(CarWheel frontWheel, CarWheel backWheel, ReactiveCommand onCarBrokenIntoTwoParts,
        LayerMask contactMask, int asphaltLayer, int groundLayer, int zombieBloodLayer)
    {
        _frontWheel = frontWheel;
        _backWheel = backWheel;
        _contactMask = contactMask;
        _asphaltLayer = asphaltLayer;
        _groundLayer = groundLayer;
        _zombieBloodLayer = zombieBloodLayer;
        _isSplittingFrame = true;
        onCarBrokenIntoTwoParts.Subscribe(_ => { CarBrokenIntoTwoParts();});
    }

    public void Dispose()
    {
        FrontWheelOnGroundReactiveProperty.Dispose();
        BackWheelOnGroundReactiveProperty.Dispose();
        FrontWheelOnAsphaltReactiveProperty.Dispose();
        BackWheelOnAsphaltReactiveProperty.Dispose();
        FrontWheelContactReactiveProperty.Dispose();
        BackWheelContactReactiveProperty .Dispose();
    }

    public void Update()
    {
        if (_isSplittingFrame == true)
        {
            CheckCircleFrontWheel();
            _isSplittingFrame = false;
        }
        else
        {
            _isSplittingFrame = true;

            if (_carBrokenIntoTwoParts == false)
            {
                CheckCircleBackWheel();
            }
        }
    }
    private void CheckCircleFrontWheel()
    {
        _frontWheelHit = Physics2D.CircleCast(_frontWheel.Position, _frontWheel.Radius,
            Vector2.down, _distance, _contactMask.value);
        if (_frontWheelHit)
        {
            CheckLayerFront();
        }
        else
        {
            SetKeyFalseFrontWheelContact();
        }
    }
    private void CheckCircleBackWheel()
    {
        _backWheelHit = Physics2D.CircleCast(_backWheel.Position, _backWheel.Radius,
            Vector2.down, _distance, _contactMask.value);
        if (_backWheelHit)
        {
            CheckLayerBack();
        }
        else
        {
            SetKeyFalseBackWheelContact();
        }
    }
    private void CheckLayerFront()
    {
        CheckLayer(FrontWheelOnGroundReactiveProperty, FrontWheelOnAsphaltReactiveProperty, FrontWheelOnZombieReactiveProperty,
            SetContactPointFrontWheelAndSetKeyTrue, _frontWheelHit);
    }
    private void CheckLayerBack()
    {
        CheckLayer(BackWheelOnGroundReactiveProperty, BackWheelOnAsphaltReactiveProperty, BackWheelOnZombieReactiveProperty,
            SetContactPointBackWheelAndSetKeyTrue, _backWheelHit);
    }
    private void CheckLayer(ReactiveProperty<bool> wheelOnGroundReactiveProperty,
        ReactiveProperty<bool> wheelOnAsphaltReactiveProperty,
        ReactiveProperty<bool> wheelOnZombieReactiveProperty,
        Action<RaycastHit2D> setContactPointWheel,
        RaycastHit2D hit)
    {
        if (hit.transform.gameObject.layer == _groundLayer)
        {
            wheelOnGroundReactiveProperty.Value = true;
            wheelOnAsphaltReactiveProperty.Value = false;
            wheelOnZombieReactiveProperty.Value = false;
            setContactPointWheel(hit);
        }
        else if(hit.transform.gameObject.layer == _asphaltLayer)
        {
            wheelOnGroundReactiveProperty.Value = false;
            wheelOnAsphaltReactiveProperty.Value = true;
            wheelOnZombieReactiveProperty.Value = false;
            setContactPointWheel(hit);
        }
        else if(hit.transform.gameObject.layer == _zombieBloodLayer)
        {
            wheelOnGroundReactiveProperty.Value = false;
            wheelOnAsphaltReactiveProperty.Value = false;
            wheelOnZombieReactiveProperty.Value = true;
            setContactPointWheel(hit);
        }
        else
        {
            wheelOnGroundReactiveProperty.Value = false;
            wheelOnAsphaltReactiveProperty.Value = false;
            wheelOnZombieReactiveProperty.Value = false;
        }
    }
    private void SetContactPointFrontWheelAndSetKeyTrue(RaycastHit2D wheelHit)
    {
        FrontWheelContactReactiveProperty.Value = true;
        FrontWheelPointContact = wheelHit.point;
    }
    private void SetContactPointBackWheelAndSetKeyTrue(RaycastHit2D wheelHit)
    {
        BackWheelContactReactiveProperty.Value = true;
        BackWheelPointContact = wheelHit.point;
    }
    private void SetKeyFalseFrontWheelContact()
    {
        FrontWheelOnGroundReactiveProperty.Value = false;
        FrontWheelOnAsphaltReactiveProperty.Value = false;
        FrontWheelContactReactiveProperty.Value = false;
    }
    private void SetKeyFalseBackWheelContact()
    {
        BackWheelOnGroundReactiveProperty.Value = false;
        BackWheelOnAsphaltReactiveProperty.Value = false;
        BackWheelContactReactiveProperty.Value = false;
    }
    private void CarBrokenIntoTwoParts()
    {
        _carBrokenIntoTwoParts = true;
    }
}