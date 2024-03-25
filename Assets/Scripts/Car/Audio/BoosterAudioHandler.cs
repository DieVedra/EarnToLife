using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class BoosterAudioHandler : AudioPlayer
{
    private readonly float _timeDelay = 0.2f;
    private readonly AudioClip _boosterStartAudioClip;
    private readonly AudioClip _boosterStopAudioClip;
    private readonly AudioClip _boosterRunAudioClip;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    public BoosterAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip boosterStartAudioClip, AudioClip boosterStopAudioClip,  AudioClip boosterRunAudioClip)
    :base(audioSource, soundReactiveProperty)
    {
        _boosterStartAudioClip = boosterStartAudioClip;
        _boosterStopAudioClip = boosterStopAudioClip;
        _boosterRunAudioClip = boosterRunAudioClip;
    }
    public async UniTaskVoid PlayBoosterRun()
    {
        StopPlay();
        TryPlayOneShotClip(_boosterStartAudioClip);
        await UniTask.Delay(TimeSpan.FromSeconds(_timeDelay), cancellationToken:_cancellationTokenSource.Token);
        TryPlayClip(_boosterRunAudioClip, true);
    }
    public void StopPlayRunBooster()
    {
        _cancellationTokenSource.Cancel();
        StopPlay();
        PlayBoosterEndFuel();
    }
    public void PlayBoosterEndFuel()
    {
        TryPlayOneShotClip(_boosterStopAudioClip);
    }
}