using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class DestructionAudioHandler : AudioPlayer
{
    private readonly float _defaultVolume = 1f;
    private readonly AudioClip _carBurnAudioClip;
    private readonly AudioClip _engineClapAudioClip;
    private readonly AudioClip _carHardHitAudioClip;
    private readonly AudioClip _carSoftHitAudioClip;
    private readonly AudioClip _glassBreakingAudioClip;
    private readonly AudioClip _metalBendsAudioClip;
    private readonly AudioClip _driverNeckBrokeAudioClip;
    private AnimationCurve _destructionAudioCurve;
    private bool _hitSoundIsPlay = false;

    public DestructionAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip carBurnAudioClip, AudioClip carHardHitAudioClip, AudioClip carSoftHitAudioClip, AudioClip glassBreakingAudioClip,
        AudioClip metalBendsAudioClip, AudioClip engineClapAudioClip, AudioClip driverNeckBrokeAudioClip)
        : base(audioSource, soundReactiveProperty)
    {
        _carBurnAudioClip = carBurnAudioClip;
        _carHardHitAudioClip = carHardHitAudioClip;
        _carSoftHitAudioClip = carSoftHitAudioClip;
        _glassBreakingAudioClip = glassBreakingAudioClip;
        _metalBendsAudioClip = metalBendsAudioClip;
        _engineClapAudioClip = engineClapAudioClip;
        _driverNeckBrokeAudioClip = driverNeckBrokeAudioClip;
    }

    public void Init(AnimationCurve destructionAudioCurve)
    {
        _destructionAudioCurve = destructionAudioCurve;
    }
    public async void PlayHardHit(float force)
    {
        if (_hitSoundIsPlay == false)
        {
            _hitSoundIsPlay = true;
            SetVolume(_destructionAudioCurve.Evaluate(force));
            TryPlayOneShotClip(_carHardHitAudioClip);
            await UniTask.Delay(TimeSpan.FromSeconds(_carHardHitAudioClip.length));
            _hitSoundIsPlay = false;
        }
    }
    public async void PlaySoftHit(float force)
    {
        if (_hitSoundIsPlay == false)
        {
            _hitSoundIsPlay = true;
            SetVolume(_destructionAudioCurve.Evaluate(force));
            TryPlayOneShotClip(_carSoftHitAudioClip);
            await UniTask.Delay(TimeSpan.FromSeconds(_carSoftHitAudioClip.length));
            _hitSoundIsPlay = false;
        }
    }
    public void PlayGlassBreak()
    {
        TryPlayOneShotClip(_glassBreakingAudioClip);

    }
    public async void PlayEngineBurn()
    {
        SetVolume(_defaultVolume);
        TryPlayOneShotClip(_engineClapAudioClip);
        await UniTask.Delay(TimeSpan.FromSeconds(_engineClapAudioClip.length));
        TryPlayClip(_carBurnAudioClip, true);
    }

    public void PlayDriverNeckBroke()
    {
        TryPlayOneShotClip(_driverNeckBrokeAudioClip);
    }
    public void PlayRoofBends(float force)
    {
        SetVolume(_defaultVolume);
        Debug.Log($"                         PlayRoofBends:                     {AudioSource.volume}");
        TryPlayClip(_metalBendsAudioClip);
    }
    public void StopPlayEngineBurn()
    {
        StopPlay();
    }
}