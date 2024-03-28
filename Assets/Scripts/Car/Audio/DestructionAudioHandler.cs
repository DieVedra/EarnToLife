using UniRx;
using UnityEngine;

public class DestructionAudioHandler : AudioPlayer
{
    private readonly float _defaultVolume = 1f;
    private readonly AudioClip _carBurnAudioClip;
    private readonly AudioClip _carHardHitAudioClip;
    private readonly AudioClip _carSoftHitAudioClip;
    private readonly AudioClip _glassBreakingAudioClip;
    private readonly AudioClip _metalBendsAudioClip;
    private AnimationCurve _destructionAudioCurve;

    public DestructionAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip carBurnAudioClip, AudioClip carHardHitAudioClip, AudioClip carSoftHitAudioClip,AudioClip glassBreakingAudioClip, AudioClip metalBendsAudioClip)
        : base(audioSource, soundReactiveProperty)
    {
        _carBurnAudioClip = carBurnAudioClip;
        _carHardHitAudioClip = carHardHitAudioClip;
        _carSoftHitAudioClip = carSoftHitAudioClip;
        _glassBreakingAudioClip = glassBreakingAudioClip;
        _metalBendsAudioClip = metalBendsAudioClip; 
    }

    public void Init(AnimationCurve destructionAudioCurve)
    {
        _destructionAudioCurve = destructionAudioCurve;
    }
    public void PlayHardHit(float force)
    {
        SetVolume(_destructionAudioCurve.Evaluate(force));
        TryPlayOneShotClip(_carHardHitAudioClip);
    }
    public void PlaySoftHit(float force)
    {
        SetVolume(_destructionAudioCurve.Evaluate(force));
        TryPlayOneShotClip(_carSoftHitAudioClip);
    }
    public void PlayGlassBreak()
    {
        TryPlayOneShotClip(_glassBreakingAudioClip);

    }
    public void PlayEngineBurn()
    {
        SetVolume(_defaultVolume);
        TryPlayClip(_carBurnAudioClip, true);
    }

    public void PlayRoofBends(float force)
    {
        SetVolume(_destructionAudioCurve.Evaluate(force));
        TryPlayClip(_metalBendsAudioClip);
    }
    public void StopPlayEngineBurn()
    {
        StopPlay();
    }
}