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
    private readonly TimeScalePitchHandler _timeScalePitchHandlerForWheelsRotate;
    private readonly TimeScalePitchHandler _timeScalePitchHandlerForSlit;
    private readonly AudioClip _wheelsRotateAudioClip;
    private readonly AudioClip _slitAudioClip;
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    public HotWheelAudioHandler(AudioSource audioSource1, AudioSource audioSource2, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        TimeScaleSignal timeScaleSignal, AudioClip wheelsRotateAudioClip, AudioClip slitAudioClip)
    {
        _audioPlayerForWheelsRotate = new AudioPlayer(audioSource1, soundReactiveProperty, audioPauseReactiveProperty);
        _audioPlayerForSlit = new AudioPlayer(audioSource2, soundReactiveProperty, audioPauseReactiveProperty);
        _timeScalePitchHandlerForWheelsRotate = new TimeScalePitchHandler(timeScaleSignal);
        _timeScalePitchHandlerForSlit = new TimeScalePitchHandler(timeScaleSignal);
        _wheelsRotateAudioClip = wheelsRotateAudioClip;
        _slitAudioClip = slitAudioClip;
        _timeScalePitchHandlerForWheelsRotate.OnPitchTimeWarped += _audioPlayerForWheelsRotate.SetPitch;
        _timeScalePitchHandlerForSlit.OnPitchTimeWarped += _audioPlayerForSlit.SetPitch;
        _timeScalePitchHandlerForWheelsRotate.IsTimeWarpedRP.Subscribe(_ =>
        {
            if (_timeScalePitchHandlerForWheelsRotate.IsTimeWarpedRP.Value == true)
            {
                _timeScalePitchHandlerForWheelsRotate.SetPitchValueNormalTimeScale(_audioPlayerForWheelsRotate.AudioSource.pitch);
            }
        });
        _timeScalePitchHandlerForSlit.IsTimeWarpedRP.Subscribe(_ =>
        {
            if (_timeScalePitchHandlerForSlit.IsTimeWarpedRP.Value == true)
            {
                _timeScalePitchHandlerForSlit.SetPitchValueNormalTimeScale(_audioPlayerForSlit.AudioSource.pitch);
            }
        });
    }

    public void Dispose()
    {
        _cancellationToken.Cancel();
        _timeScalePitchHandlerForWheelsRotate.OnPitchTimeWarped -= _audioPlayerForWheelsRotate.SetPitch;
        _timeScalePitchHandlerForSlit.OnPitchTimeWarped -= _audioPlayerForSlit.SetPitch;
        _timeScalePitchHandlerForWheelsRotate.Dispose();
        _timeScalePitchHandlerForSlit.Dispose();
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