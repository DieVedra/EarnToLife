﻿using UniRx;
using UnityEngine;

public class BrakeAudioHandler : AudioPlayer
{
    private readonly AudioClip _brakeAudioClip;

    public BrakeAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip brakeAudioClip)
        : base(audioSource, soundReactiveProperty)
    {
        _brakeAudioClip = brakeAudioClip;
    }
    public void PlayBrake()
    {
        TryPlayClip(_brakeAudioClip, true);
    }
    public void SetVolumeBrake(float volume)
    {
        AudioSource.volume = volume;
    }
    public void StopPlayBrake()
    {
        StopPlay();
    }
}