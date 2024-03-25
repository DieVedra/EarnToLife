using UniRx;
using UnityEngine;

public class GunAudioHandler : AudioPlayer
{
    private readonly AudioClip _gunAudioClip;

    public GunAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip gunAudioClip) : base(audioSource, soundReactiveProperty)
    {
        _gunAudioClip = gunAudioClip;
    }
    public void PlayShotGun()
    {
        TryPlayOneShotClip(_gunAudioClip);
    }
}