using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class HotWheelAudioHandler : AudioPlayer
{
    private readonly float _endPitchValue = 0.1f;
    private readonly float _durationPitchChange = 1.5f;
    private readonly float _audioClipsDurationMultiplier = 0.7f;
    private readonly AudioSource _audioSourceOther;
    private readonly AudioClip _wheelsRotateAudioClip;
    private readonly AudioClip _slitAudioClip;
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
    private bool _oneShotClipIsPlay = false;
    public HotWheelAudioHandler(AudioSource audioSource, AudioSource audioSourceOther, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip wheelsRotateAudioClip, AudioClip slitAudioClip) : base(audioSource, soundReactiveProperty)
    {
        _audioSourceOther = audioSourceOther;
        _wheelsRotateAudioClip = wheelsRotateAudioClip;
        _slitAudioClip = slitAudioClip;
    }

    public void Dispose()
    {
        _cancellationToken.Cancel();
    }
    public void PlayRotateWheels()
    {
        TryPlayClip(_wheelsRotateAudioClip, true);
    }

    public async UniTaskVoid StopPlayRotateWheels()
    {
        await AudioSource.DOPitch(_endPitchValue, _durationPitchChange).WithCancellation(_cancellationToken.Token);
        StopPlay();
    }

    public async UniTaskVoid TryPlayCut()
    {
        if (_oneShotClipIsPlay == false)
        {
            if (SoundOn)
            {
                Debug.Log(09090909);
                _oneShotClipIsPlay = true;
                // _audioSourceOther.PlayOneShot(_slitAudioClip);
                
                TryPlayOneShotClip(_slitAudioClip);
                await UniTask.Delay(TimeSpan.FromSeconds(_slitAudioClip.length * _audioClipsDurationMultiplier),
                    cancellationToken: _cancellationToken.Token);
            }
        }
    }
}