
using UniRx;
using UnityEngine;

public class ZombieAudioHandler: AudioPlayer
{
    private readonly AudioClip _hitZombieRemains;

    public ZombieAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip hitZombieRemains )
        :base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _hitZombieRemains = hitZombieRemains;
    }
}