﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class BarrelAudioHandler  : AudioPlayer
{
    private readonly Vector2 _volumeSection = new Vector2(0.7f, 1f);
    private readonly Vector2 _pitchSection = new Vector2(0.9f, 1.1f);
    private readonly Vector2 _volumeLerpSection = new Vector2(0f, 50f);
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly AudioClip _hitBarrelAudioClip;
    private readonly AudioClip _hit1DebrisBarrelAudioClip;
    private readonly AudioClip _hit2DebrisBarrelAudioClip;
    private readonly AudioClip _explode1BarrelAudioClip;
    private readonly AudioClip _explode2BarrelAudioClip;
    private readonly AudioClip _burnBarrelAudioClip;
    public BarrelAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip hitBarrelAudioClip, AudioClip hit1DebrisBarrelAudioClip, AudioClip hit2DebrisBarrelAudioClip,
        AudioClip explode1BarrelAudioClip, AudioClip explode2BarrelAudioClip, AudioClip burnBarrelAudioClip)
        : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _hitBarrelAudioClip = hitBarrelAudioClip;
        _hit1DebrisBarrelAudioClip = hit1DebrisBarrelAudioClip;
        _hit2DebrisBarrelAudioClip = hit2DebrisBarrelAudioClip;
        _explode1BarrelAudioClip = explode1BarrelAudioClip;
        _explode2BarrelAudioClip = explode2BarrelAudioClip;
        _burnBarrelAudioClip = burnBarrelAudioClip;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public void PlayBarrelExplosionSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(new []{_explode1BarrelAudioClip, _explode2BarrelAudioClip}),
            _volumeSection, _pitchSection);
    }
    public void PlayBarrelHitSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(new []{_hit1DebrisBarrelAudioClip, _hit2DebrisBarrelAudioClip}),
            _volumeSection, _pitchSection);
    }
    public void PlayBarrelFailBreakingSound(float force)
    {
        SetVolume(Mathf.InverseLerp(_volumeLerpSection.x, _volumeLerpSection.y, force));

        TryPlayOneShotClip(_hitBarrelAudioClip);
    }

    public async UniTaskVoid PlayBarrelBurn(float time)
    {
        TryPlayClip(_burnBarrelAudioClip);
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken:_cancellationTokenSource.Token);
        StopPlayAndSetNull();
    }
}