using UniRx;
using UnityEngine;

public class BarrelAudioHandler  : AudioPlayer
{
    private readonly Vector2 _volumeSection = new Vector2(0.7f, 1f);
    private readonly Vector2 _pitchSection = new Vector2(0.9f, 1.1f);
    private readonly AudioClip _hitBarrelAudioClip;
    private readonly AudioClip _hitDebrisBarrelAudioClip;
    private readonly AudioClip _explodeBarrelAudioClip;
    public BarrelAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip hitBarrelAudioClip, AudioClip hitDebrisBarrelAudioClip, AudioClip explodeBarrelAudioClip)
        : base(audioSource, soundReactiveProperty)
    {
        _hitBarrelAudioClip = hitBarrelAudioClip;
        _hitDebrisBarrelAudioClip = hitDebrisBarrelAudioClip;
        _explodeBarrelAudioClip = explodeBarrelAudioClip;
    }

    public void PlayBarrelExplosionSound()
    {
        // TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_explodeBarrelAudioClip, _volumeSection, _pitchSection);
    }
    public void PlayBarrelHitSound()
    {
        // TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_hitDebrisBarrelAudioClip, _volumeSection, _pitchSection);
    }
    public void PlayBarrelFailBreakingSound()
    {
        // TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_hitBarrelAudioClip, _volumeSection, _pitchSection);
    }
}