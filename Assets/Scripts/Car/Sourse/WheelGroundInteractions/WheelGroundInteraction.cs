using System;
using UniRx;
using UnityEngine;

public class WheelGroundInteraction
{
    protected readonly GroundAnalyzer GroundAnalyzer;
    protected readonly CarWheel FrontWheel;
    protected readonly CarWheel BackWheel;
    protected readonly Speedometer Speedometer;
    protected readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();
    private readonly AnimationCurve _particlesSpeedCurve;
    
    private readonly CompositeDisposable _frontWheelDirtEffectDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _backWheelDirtEffectDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _frontWheelSmokeEffectDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _backWheelSmokeEffectDisposable = new CompositeDisposable();
    protected Transform FrontWheelDirtEffectTransform => FrontWheel.DirtWheelParticleSystem.transform;
    protected Transform BackWheelDirtEffectTransform => BackWheel.DirtWheelParticleSystem.transform;
    private Transform _frontWheelSmokeEffectTransform => FrontWheel.SmokeWheelParticleSystem.transform;
    private Transform _backWheelSmokeEffectTransform => BackWheel.SmokeWheelParticleSystem.transform;
    // private bool FrontWheelOnGround => GroundAnalyzer.FrontWheelOnGroundReactiveProperty.Value;
    // private bool BackWheelOnGround => GroundAnalyzer.BackWheelOnGroundReactiveProperty.Value;
    // private bool FrontWheelOnAsphalt => GroundAnalyzer.FrontWheelOnAsphaltReactiveProperty.Value;
    // private bool BackWheelOnAsphalt => GroundAnalyzer.BackWheelOnAsphaltReactiveProperty.Value;
    
    private ParticleSystem _frontWheelDirtWheelParticleSystem => FrontWheel.DirtWheelParticleSystem;
    private ParticleSystem _backWheelDirtWheelParticleSystem => BackWheel.DirtWheelParticleSystem;
    private ParticleSystem _frontWheelSmokeWheelParticleSystem => FrontWheel.SmokeWheelParticleSystem;
    private ParticleSystem _backWheelSmokeWheelParticleSystem => BackWheel.SmokeWheelParticleSystem;
    protected WheelGroundInteraction(GroundAnalyzer groundAnalyzer, Speedometer speedometer,
        CarWheel frontWheel, CarWheel backWheel, AnimationCurve particlesSpeedCurve)
    {
        GroundAnalyzer = groundAnalyzer;
        FrontWheel = frontWheel;
        BackWheel = backWheel;
        Speedometer = speedometer;
        _particlesSpeedCurve = particlesSpeedCurve;
    }
    public virtual void Init()
    {
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnGroundReactiveProperty, PlayEffectFrontWheelOnGround);
        SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnGroundReactiveProperty, PlayEffectBackWheelOnGround);
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnAsphaltReactiveProperty, PlayEffectFrontWheelOnAsphalt);
        SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnAsphaltReactiveProperty, PlayEffectBackWheelOnAsphalt);

        SubscribeReactiveProperty(Speedometer.CurrentSpeedReactiveProperty, ChangesParticlesSpeeds);
    }
    public void Dispose()
    {
        CompositeDisposable.Clear();
        _frontWheelDirtWheelParticleSystem.Stop();
        _backWheelDirtWheelParticleSystem.Stop();
        _frontWheelSmokeWheelParticleSystem.Stop();
        _backWheelSmokeWheelParticleSystem.Stop();
    }

    public void Update()
    {
        if (GroundAnalyzer.FrontWheelContact == true)
        {
            FrontWheelDirtEffectTransform.position = GroundAnalyzer.FrontWheelPointContact;
            _frontWheelSmokeEffectTransform.position = GroundAnalyzer.FrontWheelPointContact;
        }

        if (GroundAnalyzer.BackWheelContact == true)
        {
            BackWheelDirtEffectTransform.position = GroundAnalyzer.FrontWheelPointContact;
            _backWheelSmokeEffectTransform.position = GroundAnalyzer.FrontWheelPointContact;
        }
    }
    protected virtual void SetRotation() { }
    protected void SubscribeReactiveProperty(ReactiveProperty<bool> property, Action operation)
    {
        property.Subscribe(
            _ =>
            {
                operation.Invoke();
            }).AddTo(CompositeDisposable);
    }
    private void SubscribeReactiveProperty(ReactiveProperty<bool> property, Action<bool> operation)
    {
        property.Subscribe(
            _ =>
            {
                operation.Invoke(property.Value);
            }).AddTo(CompositeDisposable);
    }
    private void SubscribeReactiveProperty(ReactiveProperty<float> property, Action operation)
    {
        property.Subscribe(
            _ =>
            {
                operation.Invoke();
            }).AddTo(CompositeDisposable);
    }
    private void PlayEffectFrontWheelOnGround(bool value)
    {
        TryPlayEffect(_frontWheelDirtWheelParticleSystem, GroundAnalyzer.FrontWheelPointContact, _frontWheelDirtEffectDisposable, value);
    }

    private void PlayEffectBackWheelOnGround(bool value)
    {
        TryPlayEffect(_backWheelDirtWheelParticleSystem, GroundAnalyzer.BackWheelPointContact, _backWheelDirtEffectDisposable, value);
    }

    private void PlayEffectFrontWheelOnAsphalt(bool value)
    {
        TryPlayEffect(_frontWheelSmokeWheelParticleSystem, GroundAnalyzer.FrontWheelPointContact, _frontWheelSmokeEffectDisposable, value);
    }

    private void PlayEffectBackWheelOnAsphalt(bool value)
    {
        TryPlayEffect(_backWheelSmokeWheelParticleSystem, GroundAnalyzer.BackWheelPointContact, _backWheelSmokeEffectDisposable, value);
    }

    private void TryPlayEffect(ParticleSystem particleSystem, Vector2 position, CompositeDisposable effectDisposable, bool value)
    {
        if (value == true)
        {
            // particleSystem.transform.localPosition = transformPoint.position;
            // particleSystem.transform.position = point;
            SubscribeEffectToUpdate(particleSystem, position, effectDisposable);
            particleSystem.Play();
        }
        else
        {
            effectDisposable.Clear();
            particleSystem.Stop();
        }
    }

    private void SubscribeEffectToUpdate(ParticleSystem particleSystem, Vector2 position, CompositeDisposable effectDisposable)
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            particleSystem.transform.position = position;
        }).AddTo(effectDisposable);
    }
    private void ChangeParticlesSpeed(ParticleSystem particleSystem, float value)
    {
        var mainModule = particleSystem.main;
        mainModule.startSpeed = value;
    }

    private void ChangesParticlesSpeeds()
    {
        float evaluateValue = _particlesSpeedCurve.Evaluate(Speedometer.CurrentSpeedFloat);
        ChangeParticlesSpeed(FrontWheel.DirtWheelParticleSystem, evaluateValue);
        ChangeParticlesSpeed(FrontWheel.SmokeWheelParticleSystem, evaluateValue);
        
        ChangeParticlesSpeed(BackWheel.DirtWheelParticleSystem, evaluateValue);
        ChangeParticlesSpeed(BackWheel.SmokeWheelParticleSystem, evaluateValue);
    }
}