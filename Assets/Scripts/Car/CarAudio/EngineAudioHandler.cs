using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class EngineAudioHandler : AudioPlayer
{
    private const float MINTIMEPITCH = 0.85f;
    private const float MAXTIMEPITCH = 1.45f;
    private readonly AudioClip _engineStartAudioClip;
    private readonly AudioClip _engineStopAudioClip;
    private readonly AudioClip _engineRunAudioClip;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public EngineAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip engineStartAudioClip, AudioClip engineStopAudioClip, AudioClip engineRunAudioClip)
    : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _engineStartAudioClip = engineStartAudioClip;
        _engineStopAudioClip = engineStopAudioClip;
        _engineRunAudioClip = engineRunAudioClip;
        PlayStartEngine().Forget();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }

    private async UniTaskVoid PlayStartEngine()
    {
        TryPlayOneShotClip(_engineStartAudioClip);
        await UniTask.Delay(TimeSpan.FromSeconds(_engineStartAudioClip.length * 0.3f), cancellationToken:_cancellationTokenSource.Token);
        PlayRun();
    }
    public void PitchControl(float value)
    {
        SetPitch(Mathf.LerpUnclamped(MINTIMEPITCH, MAXTIMEPITCH, value));
    }
    public void PlaySoundStopEngine()
    {
        // StopPlay();
        StopPlayAndSetNull();
        TryPlayOneShotClip(_engineStopAudioClip);
    }
    // public void PlaySoundSoftStopEngine()
    // {
    //     TryPlayClip(_engineSoftStopAudioClip);
    // }
    // public void PlaySoundExplosionEngine()
    // {
    //     TryPlayClip(_engineExplosionAudioClip);
    // }
    public void StopPlayEngine()
    {
        StopPlay();
    }

    private void PlayRun()
    {
        TryPlayClip(_engineRunAudioClip);
    }
}