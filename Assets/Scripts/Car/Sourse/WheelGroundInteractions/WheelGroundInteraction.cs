﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class WheelGroundInteraction
{
    protected readonly GroundAnalyzer GroundAnalyzer;
    protected readonly CompositeDisposable CompositeDisposableFrontWheel = new CompositeDisposable();
    protected readonly CompositeDisposable CompositeDisposableBackWheel = new CompositeDisposable();
    protected readonly Speedometer Speedometer;
    private readonly float _timeDelayDisable = 2f;
    private readonly CarWheel _frontWheel;
    private readonly CarWheel _backWheel;
    private readonly AnimationCurve _particlesSpeedCurve;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly ParticleSystem[] _effects;
    private readonly ParticleSystem[] _backWheelEffects;


    private readonly ParticleSystem _frontWheelDirtWheelParticleSystem;
    private readonly ParticleSystem _backWheelDirtWheelParticleSystem;
    private readonly ParticleSystem _frontWheelSmokeWheelParticleSystem;
    private readonly ParticleSystem _backWheelSmokeWheelParticleSystem;
    private readonly ParticleSystem _frontWheelZombieBloodParticleSystem;
    private readonly ParticleSystem _backWheelZombieBloodParticleSystem;

    private readonly Transform _frontWheelSmokeEffectTransform;
    private readonly Transform _backWheelSmokeEffectTransform;
    private readonly Transform _frontWheelZombieBloodEffectTransform;
    private readonly Transform _backWheelZombieBloodEffectTransform;
    protected readonly Transform FrontWheelDirtEffectTransform;
    protected readonly Transform BackWheelDirtEffectTransform;
    private float _evaluatedValue;

    protected WheelGroundInteraction(GroundAnalyzer groundAnalyzer, Speedometer speedometer,
        CarWheel frontWheel, CarWheel backWheel, AnimationCurve particlesSpeedCurve, ReactiveCommand onCarBrokenIntoTwoParts)
    {
        GroundAnalyzer = groundAnalyzer;
        _frontWheel = frontWheel;
        _backWheel = backWheel;
        Speedometer = speedometer;
        _particlesSpeedCurve = particlesSpeedCurve;
        
        FrontWheelDirtEffectTransform = _frontWheel.DirtWheelParticleSystem.transform;
        BackWheelDirtEffectTransform = _backWheel.DirtWheelParticleSystem.transform;
        _frontWheelSmokeEffectTransform = _frontWheel.SmokeWheelParticleSystem.transform;
        _backWheelSmokeEffectTransform = _backWheel.SmokeWheelParticleSystem.transform;
        _frontWheelZombieBloodEffectTransform = _frontWheel.BloodWheelParticleSystem.transform;
        _backWheelZombieBloodEffectTransform = _backWheel.BloodWheelParticleSystem.transform;
        
        _frontWheelDirtWheelParticleSystem = _frontWheel.DirtWheelParticleSystem;
        _backWheelDirtWheelParticleSystem = _backWheel.DirtWheelParticleSystem;
        _frontWheelSmokeWheelParticleSystem = _frontWheel.SmokeWheelParticleSystem;
        _backWheelSmokeWheelParticleSystem = _backWheel.SmokeWheelParticleSystem;
        _frontWheelZombieBloodParticleSystem = _frontWheel.BloodWheelParticleSystem;
        _backWheelZombieBloodParticleSystem = _backWheel.BloodWheelParticleSystem;
        
        _effects = new[]
        {
            _frontWheelDirtWheelParticleSystem, _backWheelDirtWheelParticleSystem,
            _frontWheelSmokeWheelParticleSystem, _backWheelSmokeWheelParticleSystem,
            _frontWheelZombieBloodParticleSystem, _backWheelZombieBloodParticleSystem
        };
        _backWheelEffects = new[]
        {
            _backWheelDirtWheelParticleSystem, _backWheelSmokeWheelParticleSystem, _backWheelZombieBloodParticleSystem
        };
        onCarBrokenIntoTwoParts.Subscribe(_ => { CarBrokenIntoTwoParts();});
    }

    public virtual void Init(bool carBroken)
    {
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnGroundReactiveProperty, PlayEffectFrontWheelOnGround, CompositeDisposableFrontWheel);
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnAsphaltReactiveProperty, PlayEffectFrontWheelOnAsphalt, CompositeDisposableFrontWheel);
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnZombieReactiveProperty, PlayEffectFrontWheelOnZombie, CompositeDisposableFrontWheel);
        if (carBroken == false)
        {
            SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnGroundReactiveProperty, PlayEffectBackWheelOnGround, CompositeDisposableBackWheel);
            SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnAsphaltReactiveProperty, PlayEffectBackWheelOnAsphalt, CompositeDisposableBackWheel);
            SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnZombieReactiveProperty, PlayEffectBackWheelOnZombie, CompositeDisposableBackWheel);
        }
        EnableEffects();
    }

    public void Dispose()
    {
        CompositeDisposableFrontWheel.Clear();
        CompositeDisposableBackWheel.Clear();
        _cancellationTokenSource.Cancel();
        StopAndDisableEffects(_effects);
    }

    public void Update()
    {
        if (GroundAnalyzer.FrontWheelContact == true)
        {
            FrontWheelDirtEffectTransform.position = GroundAnalyzer.FrontWheelPointContact;
            _frontWheelSmokeEffectTransform.position = GroundAnalyzer.FrontWheelPointContact;
            _frontWheelZombieBloodEffectTransform.position = GroundAnalyzer.FrontWheelPointContact;
        }
        if (GroundAnalyzer.BackWheelContact == true)
        {
            BackWheelDirtEffectTransform.position = GroundAnalyzer.BackWheelPointContact;
            _backWheelSmokeEffectTransform.position = GroundAnalyzer.BackWheelPointContact;
            _backWheelZombieBloodEffectTransform.position = GroundAnalyzer.BackWheelPointContact;
        }
    }
    protected virtual void SetRotation() { }

    protected void StopEffects()
    {
        foreach (var effect in _effects)
        {
            effect.Stop();
        }
    }
    protected virtual void ChangesParticlesSpeeds()
    {
        EvaluateCurve();
        ChangeParticlesSpeed(_frontWheelDirtWheelParticleSystem, _evaluatedValue);
        ChangeParticlesSpeed(_frontWheelSmokeWheelParticleSystem, _evaluatedValue);
        ChangeParticlesSpeed(_frontWheelZombieBloodParticleSystem, _evaluatedValue);
        
        ChangeParticlesSpeed(_backWheelDirtWheelParticleSystem, _evaluatedValue);
        ChangeParticlesSpeed(_backWheelSmokeWheelParticleSystem, _evaluatedValue);
        ChangeParticlesSpeed(_backWheelZombieBloodParticleSystem, _evaluatedValue);
    }

    protected void SubscribeReactiveProperty(ReactiveProperty<bool> property, Action operation, CompositeDisposable compositeDisposable)
    {
        property.Subscribe(
            _ =>
            {
                operation.Invoke();
            }).AddTo(compositeDisposable);
    }

    protected void SubscribeReactiveProperty(ReactiveProperty<float> property, Action operation, CompositeDisposable compositeDisposable)
    {
        property.Subscribe(
            _ =>
            {
                operation.Invoke();
            }).AddTo(compositeDisposable);
    }

    private void SubscribeReactiveProperty(ReactiveProperty<bool> property, Action<bool> operation, CompositeDisposable compositeDisposable)
    {
        property.Subscribe(
            _ =>
            {
                operation.Invoke(property.Value);
            }).AddTo(compositeDisposable);
    }

    private void PlayEffectFrontWheelOnGround(bool value)
    {
        TryPlayEffect(_frontWheelDirtWheelParticleSystem, value);
    }

    private void PlayEffectBackWheelOnGround(bool value)
    {
        TryPlayEffect(_backWheelDirtWheelParticleSystem, value);
    }

    private void PlayEffectFrontWheelOnAsphalt(bool value)
    {
        TryPlayEffect(_frontWheelSmokeWheelParticleSystem, value);
    }

    private void PlayEffectBackWheelOnAsphalt(bool value)
    {
        TryPlayEffect(_backWheelSmokeWheelParticleSystem, value);
    }
    private void PlayEffectFrontWheelOnZombie(bool value)
    {
        TryPlayEffect(_frontWheelZombieBloodParticleSystem, value);
    }

    private void PlayEffectBackWheelOnZombie(bool value)
    {
        TryPlayEffect(_backWheelZombieBloodParticleSystem, value);
    }
    private void TryPlayEffect(ParticleSystem particleSystem, bool value)
    {
        if (value == true)
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }

    private void EvaluateCurve()
    {
        _evaluatedValue = _particlesSpeedCurve.Evaluate(Speedometer.CurrentSpeedFloat);
    }
    private void ChangeParticlesSpeed(ParticleSystem particleSystem, float value)
    {
        var mainModule = particleSystem.main;
        mainModule.startSpeed = value;
    }

    private void StopAndDisableEffects(ParticleSystem[] effects)
    {
        foreach (var effect in effects)
        {
            StopAndDisableEffectWithDelay(effect).Forget();
        }
    }

    private void EnableEffects()
    {
        _cancellationTokenSource.Cancel();
        foreach (var effect in _effects)
        {
            effect.gameObject.SetActive(true);
        }
    }
    private async UniTaskVoid StopAndDisableEffectWithDelay(ParticleSystem particleSystem)
    {
        particleSystem.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(_timeDelayDisable), cancellationToken: _cancellationTokenSource.Token);
        particleSystem.gameObject.SetActive(false);
    }
    private void CarBrokenIntoTwoParts()
    {
        CompositeDisposableBackWheel.Clear();
        StopAndDisableEffects(_backWheelEffects);
    }
}