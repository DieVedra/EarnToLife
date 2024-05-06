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
    private readonly AudioClip _boosterEndFuel;
    private readonly AnimationCurve _increaseBoosterSoundCurve;
    private readonly AnimationCurve _decreaseBoosterSoundCurve;
    private float _currentAudioValue;
    private float _currentEvaluateCurveValue;
    public bool VolumeIncreaseValue = false;
    public bool VolumeDecreaseValue = false;
    public BoosterAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip boosterRunAudioClip, AudioClip boosterEndFuel,
        AnimationCurve increaseBoosterSoundCurve, AnimationCurve decreaseBoosterSoundCurve)
    :base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _boosterRunAudioClip = boosterRunAudioClip;
        _boosterEndFuel = boosterEndFuel;
        _increaseBoosterSoundCurve = increaseBoosterSoundCurve;
        _decreaseBoosterSoundCurve = decreaseBoosterSoundCurve;
        _currentAudioValue = _startIncreaseValue;
    }
    public void SetAudioIncreaseBooster()
    {
        TryPlayClip(_boosterRunAudioClip);
        VolumeIncreaseValue = true;
        VolumeDecreaseValue = false;
    }
    public void SetDecreaseBooster()
    {
        VolumeDecreaseValue = true;
        VolumeIncreaseValue = false;
    }
    public void StopPlayRunBoosterImmediately()
    {
        VolumeDecreaseValue = false;
        VolumeIncreaseValue = false;
        StopPlayAndSetNull();
    }
    public void VolumeIncrease() //++
    {
        if (_currentAudioValue < _startDecreaseValue)
        {
            _currentAudioValue += Time.deltaTime;
            PitchControl(GetIncreaseValue());
        }
        else
        {
            VolumeIncreaseValue = false;
        }
    }
    public void VolumeDecrease() // --
    {
        if (_currentAudioValue > _startIncreaseValue)
        {
            _currentAudioValue -= Time.deltaTime;
            PitchControl(GetDecreaseValue());
        }
        else
        {
            VolumeDecreaseValue = false;
            StopPlayAndSetNull();
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

    public void PlayBoosterEndFuel()
    {
        TryPlayOneShotClip(_boosterEndFuel);
    }
}