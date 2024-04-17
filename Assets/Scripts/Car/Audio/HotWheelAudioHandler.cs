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
    private readonly float _audioClipsDurationMultiplier = 0.3f;
    private readonly AudioPlayer _audioPlayerForWheelsRotate;
    private readonly AudioPlayer _audioPlayerForSlit;
    private readonly AudioClip _wheelsRotateAudioClip;
    private readonly AudioClip _slitAudioClip;
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    private bool _oneShotClipIsPlay = false;
    public HotWheelAudioHandler(AudioSource audioSource1, AudioSource audioSource2, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip wheelsRotateAudioClip, AudioClip slitAudioClip)
    {
        _audioPlayerForWheelsRotate = new AudioPlayer(audioSource1, soundReactiveProperty);
        _audioPlayerForSlit = new AudioPlayer(audioSource2, soundReactiveProperty);
        _wheelsRotateAudioClip = wheelsRotateAudioClip;
        _slitAudioClip = slitAudioClip;
    }

    public void Dispose()
    {
        _cancellationToken.Cancel();
    }
    public void PlayRotateWheels()
    {
        _audioPlayerForWheelsRotate.TryPlayClip(_wheelsRotateAudioClip, true);
    }

    public async UniTaskVoid StopPlayRotateWheels()
    {
        Debug.Log($"StopPlayRotateWheels");
        await _audioPlayerForWheelsRotate.AudioSource.DOPitch(_endPitchValue, _durationPitchChange).WithCancellation(_cancellationToken.Token);
        _audioPlayerForWheelsRotate.StopPlay();
    }

    // public async UniTaskVoid TryPlayCut()
    // {
    //     if (_oneShotClipIsPlay == false)
    //     {
    //         if (SoundOn)
    //         {
    //             Debug.Log(09090909);
    //             _oneShotClipIsPlay = true;
    //             TryPlayOneShotClip(_slitAudioClip);
    //             await UniTask.Delay(TimeSpan.FromSeconds(_slitAudioClip.length * _audioClipsDurationMultiplier),
    //                 cancellationToken: _cancellationToken.Token);
    //             _oneShotClipIsPlay = false;
    //
    //         }
    //     }
    // }
    public void TryPlayCut()
    {
        _audioPlayerForSlit.TryPlayOneShotClip(_slitAudioClip);
    }
}