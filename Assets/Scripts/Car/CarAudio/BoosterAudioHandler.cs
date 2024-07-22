using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class BoosterAudioHandler : AudioPlayer
{
    private readonly float _pitchMin = 0.65f;
    private readonly float _pitchMax = 1.35f;
    private readonly AudioClip _boosterRunAudioClip;
    private readonly AudioClip _boosterEndFuel;
    public BoosterAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip boosterRunAudioClip, AudioClip boosterEndFuel)
    :base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _boosterRunAudioClip = boosterRunAudioClip;
        _boosterEndFuel = boosterEndFuel;
    }
    public void PlayRunBooster()
    {
        TryPlayClip(_boosterRunAudioClip);
    }
    public void StopPlayRunBoosterImmediately()
    {
        StopPlayAndSetNull();
    }
    public void PitchControl(ref float value)
    {
        SetPitch(Mathf.LerpUnclamped(_pitchMin, _pitchMax, value * Time.timeScale));
    }

    public void PlayBoosterEndFuel()
    {
        StopPlayAndSetNull();
        TryPlayOneShotClip(_boosterEndFuel);
    }
}