using UniRx;
using UnityEngine;

public class WoodDestructibleAudioHandler : AudioPlayer
{
    private readonly Vector2 _volumeSection = new Vector2(0.7f, 1f);
    private readonly Vector2 _volumeLerpSection = new Vector2(0f, 100f);
    private readonly Vector2 _pitchSection = new Vector2(0.9f, 1.1f);
    private readonly AudioClip _woodBreakingAudioClips;
    private readonly AudioClip _woodNotBreakingAudioClips;
    private readonly AudioClip[] _hitWoodAudioClips;
    private float _previousVolume;
    private float _volumeForce;

    public WoodDestructibleAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip woodBreakingAudioClip, AudioClip woodNotBreaking2AudioClip,
        AudioClip hitWood1AudioClip, AudioClip hitWood2AudioClip, AudioClip hitWood3AudioClip) 
        : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _hitWoodAudioClips = new[] {hitWood1AudioClip, hitWood2AudioClip, hitWood3AudioClip};
        _woodBreakingAudioClips = woodBreakingAudioClip;
        _woodNotBreakingAudioClips = woodNotBreaking2AudioClip;
    }
    public void PlayWoodBreakingSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_woodBreakingAudioClips, _volumeSection, _pitchSection);
    }

    public void PlayWoodNotBreakingSound(float force)
    {
        SetVolume(Mathf.InverseLerp(_volumeLerpSection.x, _volumeLerpSection.y, force));
        TryPlayOneShotClip(_woodNotBreakingAudioClips);
    }
    public void PlayHitWoodSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(_hitWoodAudioClips), _volumeSection, _pitchSection);

    }
}