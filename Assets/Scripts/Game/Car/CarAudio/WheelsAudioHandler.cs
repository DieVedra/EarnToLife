﻿using UniRx;
using UnityEngine;

public class WheelsAudioHandler : AudioPlayer
{
    private readonly float _volumeDefault = 0.01f;
    private readonly float _deltaTimeMultiplier = 0.33f;
    private readonly TimeScaleSignal _timeScaleSignal;
    private readonly AudioClip _wheelHitAudioClip;
    private readonly TimeScalePitchHandler _timeScalePitchHandler;
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private GroundAnalyzer _groundAnalyzer;
    private float _wheelsVolume;
    private bool _previousFrontWheelContactValue;
    private bool _previousBackWheelContactValue;
    private bool _timerOn = false;
    private ReactiveProperty<bool> _frontWheelContactReactiveProperty => _groundAnalyzer.FrontWheelContactReactiveProperty;
    private ReactiveProperty<bool> _backWheelContactReactiveProperty => _groundAnalyzer.BackWheelContactReactiveProperty;
    public BrakeAudioHandler BrakeAudioHandler { get; private set; }
    public WheelsAudioHandler(AudioSource forWheelsFriction, AudioSource forWheelsHit, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        TimeScaleSignal timeScaleSignal, AudioClip brakeAudioClip, AudioClip brake2AudioClip, AudioClip wheelHitAudioClip, AnimationCurve brakeVolumeCurve)
        : base(forWheelsHit, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _timeScalePitchHandler = new TimeScalePitchHandler(timeScaleSignal);
        _wheelHitAudioClip = wheelHitAudioClip;
        BrakeAudioHandler = new BrakeAudioHandler(forWheelsFriction, soundReactiveProperty, audioPauseReactiveProperty, brakeAudioClip, brake2AudioClip, brakeVolumeCurve);
        SetVolume(_volumeDefault);
        _timeScalePitchHandler.OnPitchTimeWarped += SetPitch;
        _timeScalePitchHandler.IsTimeWarpedRP.Subscribe(_ =>
        {
            if (_timeScalePitchHandler.IsTimeWarpedRP.Value == true)
            {
                _timeScalePitchHandler.SetPitchValueNormalTimeScale(AudioSource.pitch);
            }
        });
    }

    public void Dispose()
    {
        TryStopTimer();
        _timeScalePitchHandler.OnPitchTimeWarped -= SetPitch;
    }
    public void Init(GroundAnalyzer groundAnalyzer)
    {
        _groundAnalyzer = groundAnalyzer;
        _frontWheelContactReactiveProperty.Subscribe(_ =>{ FrontWheelContactValueChanged(); });
        _backWheelContactReactiveProperty.Subscribe(_ => { BackWheelContactValueChanged(); });
    }

    private void FrontWheelContactValueChanged()
    {
        WheelContactValueChanged(_frontWheelContactReactiveProperty, ref _previousFrontWheelContactValue);
        _previousFrontWheelContactValue = _frontWheelContactReactiveProperty.Value;
    }
    private void BackWheelContactValueChanged()
    {
        WheelContactValueChanged(_backWheelContactReactiveProperty, ref _previousBackWheelContactValue);
        _previousBackWheelContactValue = _backWheelContactReactiveProperty.Value;
    }
    
    private void WheelContactValueChanged(ReactiveProperty<bool> property, ref bool previousValue)
    {
        if (property.Value == true && previousValue == false)
        {
            TryStopTimer();
            SetVolume(_wheelsVolume);
            TryPlayOneShotClip(_wheelHitAudioClip);
        }
        else if(property.Value == false)
        {
            _wheelsVolume = _volumeDefault;
            TryStartTimer();
        }
    }
    private void TryStartTimer()
    {
        if (_timerOn == false)
        {
            _timerOn = true;
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (_wheelsVolume < 1f)
                {
                    _wheelsVolume += Time.deltaTime * _deltaTimeMultiplier;
                }
                else
                {
                    TryStopTimer();
                }
            }).AddTo(_compositeDisposable);
        }
    }
    private void TryStopTimer()
    {
        _compositeDisposable.Clear();
        _timerOn = false;
    }
}