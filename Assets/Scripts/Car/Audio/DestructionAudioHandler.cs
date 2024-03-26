using UniRx;
using UnityEngine;

public class DestructionAudioHandler : AudioPlayer
{
    private readonly AudioClip _carBurnAudioClip;
    private readonly AudioClip _carHitAudioClip;
    private readonly AudioClip _glassBreakingAudioClip;
    
    public DestructionAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip carBurnAudioClip, AudioClip carHitAudioClip, AudioClip glassBreakingAudioClip)
        : base(audioSource, soundReactiveProperty)
    {
        _carBurnAudioClip = carBurnAudioClip;
        _carHitAudioClip = carHitAudioClip;
        _glassBreakingAudioClip = glassBreakingAudioClip;
    }

    public void PlayHit()
    {
        TryPlayOneShotClip(_carHitAudioClip);
    }

    public void PlayGlassBreak()
    {
        TryPlayOneShotClip(_glassBreakingAudioClip);

    }
    public void PlayEngineBurn()
    {
        TryPlayClip(_carBurnAudioClip);
    }

    public void StopPlayEngineBurn()
    {
        StopPlay();
    }
}