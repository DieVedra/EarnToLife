using UniRx;
using UnityEngine;

public class DestructionAudioHandler : AudioPlayer
{
    private readonly AudioClip _carBurnAudioClip;
    private readonly AudioClip _carHit1AudioClip;
    private readonly AudioClip _carHit2AudioClip;
    private readonly AudioClip _glassBreakingAudioClip;
    private readonly AudioClip _metalBendsAudioClip;
    
    public DestructionAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip carBurnAudioClip, AudioClip carHit1AudioClip, AudioClip carHit2AudioClip,AudioClip glassBreakingAudioClip, AudioClip metalBendsAudioClip)
        : base(audioSource, soundReactiveProperty)
    {
        _carBurnAudioClip = carBurnAudioClip;
        _carHit1AudioClip = carHit1AudioClip;
        _carHit2AudioClip = carHit2AudioClip;
        _glassBreakingAudioClip = glassBreakingAudioClip;
        _metalBendsAudioClip = metalBendsAudioClip;
    }

    public void PlayHit1()
    {
        TryPlayOneShotClip(_carHit1AudioClip);
        // Debug.Log($"                PlayHit1");
    }
    public void PlayHit2()
    {
        TryPlayOneShotClip(_carHit2AudioClip);
        // Debug.Log($"                PlayHit2");
    }
    public void PlayGlassBreak()
    {
        TryPlayOneShotClip(_glassBreakingAudioClip);

    }
    public void PlayEngineBurn()
    {
        TryPlayClip(_carBurnAudioClip);
    }

    public void PlayRoofBends()
    {
        TryPlayClip(_metalBendsAudioClip);
    }
    public void StopPlayEngineBurn()
    {
        StopPlay();
    }
}