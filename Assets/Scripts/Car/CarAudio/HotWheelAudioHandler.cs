using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class HotWheelAudioHandler
{
    private readonly float _endPitchValue = 0.1f;
    private readonly float _durationPitchChange = 1.5f;
    private readonly AudioPlayer _audioPlayerForWheelsRotate;
    private readonly AudioPlayer _audioPlayerForSlit;
    private readonly AudioClip _wheelsRotateAudioClip;
    private readonly AudioClip _slitAudioClip;
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    public HotWheelAudioHandler(AudioSource audioSource1, AudioSource audioSource2, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip wheelsRotateAudioClip, AudioClip slitAudioClip)
    {
        _audioPlayerForWheelsRotate = new AudioPlayer(audioSource1, soundReactiveProperty, audioPauseReactiveProperty);
        _audioPlayerForSlit = new AudioPlayer(audioSource2, soundReactiveProperty, audioPauseReactiveProperty);
        _wheelsRotateAudioClip = wheelsRotateAudioClip;
        _slitAudioClip = slitAudioClip;
    }

    public void Dispose()
    {
        _cancellationToken.Cancel();
    }
    public void PlayRotateWheels()
    {
        _audioPlayerForWheelsRotate.TryPlayClip(_wheelsRotateAudioClip);
    }

    public async UniTaskVoid StopPlaySoundRotateWheels()
    {
        await _audioPlayerForWheelsRotate.AudioSource.DOPitch(_endPitchValue, _durationPitchChange).WithCancellation(_cancellationToken.Token);
        _audioPlayerForWheelsRotate.StopPlay();
    }
    public void TryPlayCut()
    {
        _audioPlayerForSlit.TryPlayOneShotClip(_slitAudioClip);
    }
}