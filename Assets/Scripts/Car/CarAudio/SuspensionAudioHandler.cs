using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

public class SuspensionAudioHandler
{
    private readonly float _delay = 0.3f;
    private readonly AudioPlayer _audioPlayerFrontSuspension;
    private readonly AudioPlayer _audioPlayerBackSuspension;
    private readonly AudioClip _suspensionAudioClip;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private GroundAnalyzer _groundAnalyzer;
    private float _speedValueToPitch;
    private float _arithmeticMean;
    private float _pitchValue;
    public SuspensionAudioHandler(AudioSource carAudioSourceFrontSuspension, AudioSource carAudioSourceBackSuspension,
        ReactiveProperty<bool> soundReactiveProperty,
        AudioClip suspensionAudioClip)
    {
        _audioPlayerFrontSuspension = new AudioPlayer(carAudioSourceFrontSuspension, soundReactiveProperty);
        _audioPlayerBackSuspension = new AudioPlayer(carAudioSourceBackSuspension, soundReactiveProperty);
        _suspensionAudioClip = suspensionAudioClip;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }

    public void CalculateVolumeFrontSuspension(float value)
    {
        if (_groundAnalyzer.FrontWheelContact == true)
        {
            
            _audioPlayerFrontSuspension.Play();
            _audioPlayerFrontSuspension.SetVolume(value);
        }
        else
        {
            _audioPlayerFrontSuspension.Pause();
        }
    }

    public void CalculateVolumeBackSuspension(float value)
    {
        if (_groundAnalyzer.BackWheelContact == true)
        {
            
            _audioPlayerBackSuspension.Play();
            _audioPlayerBackSuspension.SetVolume(value);
        }
        else
        {
            _audioPlayerBackSuspension.Pause();
        }
    }

    public async UniTaskVoid Init(GroundAnalyzer groundAnalyzer)
    {
        _groundAnalyzer = groundAnalyzer;
        _audioPlayerFrontSuspension.TryPlayClip(_suspensionAudioClip, true);
        _audioPlayerFrontSuspension.SetVolume(0f);
        _audioPlayerBackSuspension.SetVolume(0f);
        await UniTask.Delay(TimeSpan.FromSeconds( _delay), cancellationToken: _cancellationTokenSource.Token);
        _audioPlayerBackSuspension.TryPlayClip(_suspensionAudioClip, true);
    }
}