using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

public class SuspensionAudioHandler
{
    private readonly Vector2 _pitchRange = new Vector2(0.85f, 1.15f);
    private readonly Vector2 _speedRange = new Vector2(0f, 50f);
    private readonly float _delay = 0.3f;
    private readonly float _halfMultiplier = 0.5f;
    private readonly AnimationCurve _animationCurveToSpeedValue;
    private readonly AudioPlayer _audioPlayerFrontSuspension;
    private readonly AudioPlayer _audioPlayerBackSuspension;
    private readonly AudioClip _suspensionAudioClip;
    private readonly AudioClip _frictionAudioClip;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private GroundAnalyzer _groundAnalyzer;
    private Speedometer _speedometer;
    private float _speedValueToPitch;
    private float _arithmeticMean;
    private float _pitchValue;
    public SuspensionAudioHandler(AudioSource carAudioSourceFrontSuspension, AudioSource carAudioSourceBackSuspension, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip suspensionAudioClip, AudioClip frictionAudioClip,
        
        AnimationCurve animationCurveToSpeedValue)
    {
        _audioPlayerFrontSuspension = new AudioPlayer(carAudioSourceFrontSuspension, soundReactiveProperty);
        _audioPlayerBackSuspension = new AudioPlayer(carAudioSourceBackSuspension, soundReactiveProperty);
        _suspensionAudioClip = suspensionAudioClip;
        _frictionAudioClip = frictionAudioClip;
        
        
        _animationCurveToSpeedValue = animationCurveToSpeedValue;
        // _animationCurveToSpeedValue = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        // _animationCurveToSpeedValue = new AnimationCurve(
        //     new Keyframe(0f, 0f),
        //     new Keyframe(0.2f,0.8f),
        //     new Keyframe(1f, 1f));
        // _animationCurveToSpeedValue.SmoothTangents(1, -1);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public async UniTaskVoid Init(GroundAnalyzer groundAnalyzer, Speedometer speedometer)
    {
        _groundAnalyzer = groundAnalyzer;
        _speedometer = speedometer;
        _audioPlayerFrontSuspension.TryPlayClip(_suspensionAudioClip, true);
        _audioPlayerFrontSuspension.SetVolume(0f);
        _audioPlayerBackSuspension.SetVolume(0f);
        await UniTask.Delay(TimeSpan.FromSeconds( _delay), cancellationToken: _cancellationTokenSource.Token);
        _audioPlayerBackSuspension.TryPlayClip(_suspensionAudioClip, true);
    }
    public void CalculateVolumeAndPitchFrontSuspension(float value)
    {
        if (_groundAnalyzer.FrontWheelContact == true)
        {
            
            _audioPlayerFrontSuspension.Play();
            CalculateVolumeAndPitch(_audioPlayerFrontSuspension, ref value);
        }
        else
        {
            _audioPlayerFrontSuspension.Pause();
        }
    }
    public void CalculateVolumeAndPitchBackSuspension(float value)
    {
        // if (_groundAnalyzer.BackWheelContact && value <= 1f)
        // {
        //     CalculateVolumeAndPitch(_audioPlayerBackSuspension, ref value);
        // }
        // else
        // {
        //     
        // }
    }


    private void CalculateVolumeAndPitch(AudioPlayer audioPlayer, ref float valueSuspension)
    {
        Debug.Log(2);
        _speedValueToPitch = Mathf.InverseLerp(_speedRange.x, _speedRange.y, _speedometer.CurrentSpeedFloat);
        // _arithmeticMean = (valueSuspension + _speedValueToPitch) *_halfMultiplier;
        // _pitchValue = Mathf.Lerp(_pitchRange.x, _pitchRange.y, _arithmeticMean);
        _pitchValue = Mathf.Lerp(_pitchRange.x, _pitchRange.y, _speedValueToPitch);
        audioPlayer.SetPitch(_pitchValue);
        audioPlayer.SetVolume(valueSuspension);
    }
}