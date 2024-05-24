
using UniRx;
using UnityEngine;

public class ZombieAudioHandler: AudioPlayer
{
    public ZombieAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty)
        :base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        
    }
}