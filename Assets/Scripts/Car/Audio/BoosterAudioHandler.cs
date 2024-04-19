using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class BoosterAudioHandler : AudioPlayer
{
    private readonly float _pitchMin = 0.65f;
    private readonly float _pitchMax = 1.35f;
    private readonly float _startIncreaseValue = 0f;
    private readonly float _startDecreaseValue = 1f;
    private readonly AudioClip _boosterRunAudioClip;
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private AnimationCurve _increaseBoosterSoundCurve;
    private AnimationCurve _decreaseBoosterSoundCurve;
    private float _currentAudioValue;
    private float _currentEvaluateCurveValue;
    public BoosterAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, AudioClip boosterRunAudioClip)
    :base(audioSource, soundReactiveProperty)
    {
        _boosterRunAudioClip = boosterRunAudioClip;
    }

    public void Init(AnimationCurve increaseBoosterSoundCurve, AnimationCurve decreaseBoosterSoundCurve)
    {
        _increaseBoosterSoundCurve = increaseBoosterSoundCurve;
        _decreaseBoosterSoundCurve = decreaseBoosterSoundCurve;
    }
    public void PlayRunBooster()
    {
        _currentAudioValue = _startIncreaseValue;
        _compositeDisposable.Clear();
        TryPlayClip(_boosterRunAudioClip, true);
        Observable.EveryUpdate().Subscribe(_ =>
        {
            VolumeIncrease();
        }).AddTo(_compositeDisposable);
    }
    public void StopPlayRunBoosterDecrease()
    {
        StopPlayRunBooster();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            VolumeDecrease();
        }).AddTo(_compositeDisposable);
    }
    public void StopPlayRunBooster()
    {
        _currentAudioValue = _startDecreaseValue;
        _compositeDisposable.Clear();
    }
    private void VolumeIncrease() //++
    {
        if (_currentAudioValue < _startDecreaseValue)
        {
            _currentAudioValue += Time.deltaTime;
            PitchControl(GetIncreaseValue());
        }
        else
        {
            _compositeDisposable.Clear();
        }
    }

    private void VolumeDecrease() // --
    {
        if (_currentAudioValue > _startIncreaseValue)
        {
            _currentAudioValue -= Time.deltaTime;
            PitchControl(GetDecreaseValue());
        }
        else
        {
            _compositeDisposable.Clear();
            StopPlay();
        }
    }

    private float GetIncreaseValue()
    {
        return _increaseBoosterSoundCurve.Evaluate(_currentAudioValue);
    }
    private float GetDecreaseValue()
    {
        return _decreaseBoosterSoundCurve.Evaluate(_currentAudioValue);
    }
    private void PitchControl(float value)
    {
        SetPitch(Mathf.LerpUnclamped(_pitchMin, _pitchMax, value));
    }
}