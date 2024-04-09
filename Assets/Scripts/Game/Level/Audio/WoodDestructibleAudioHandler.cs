using UniRx;
using UnityEngine;

public class WoodDestructibleAudioHandler : AudioPlayer
{
    private readonly Vector2 _volumeSection = new Vector2(0.7f, 1f);
    private readonly Vector2 _pitchSection = new Vector2(0.9f, 1.1f);
    private readonly AudioClip _woodBreakingAudioClips;
    private readonly AudioClip _woodNotBreakingAudioClips;
    private readonly AudioClip[] _hitWoodAudioClips;
    private float _previousVolume;

    public WoodDestructibleAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip woodBreakingAudioClip, AudioClip woodNotBreaking2AudioClip,
        AudioClip hitWood1AudioClip, AudioClip hitWood2AudioClip, AudioClip hitWood3AudioClip) 
        : base(audioSource, soundReactiveProperty)
    {
        _hitWoodAudioClips = new[] {hitWood1AudioClip, hitWood2AudioClip, hitWood3AudioClip};
        _woodBreakingAudioClips = woodBreakingAudioClip;
        _woodNotBreakingAudioClips = woodNotBreaking2AudioClip;
    }

    public void PlayWoodBreakingSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_woodBreakingAudioClips, _volumeSection, _pitchSection);
    }

    public void PlayWoodNotBreakingSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_woodNotBreakingAudioClips, _volumeSection, _pitchSection);
    }
    public void PlayHitWoodSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(_hitWoodAudioClips), _volumeSection, _pitchSection);
    }
}