using UniRx;
using UnityEngine;

public class ZombieBoozerAudioHandler : AudioPlayer
{
    private readonly Vector2 _pitchSection = new Vector2(0.9f,1.1f);
    private readonly Vector2 _volumeSection = new Vector2(0.7f,1f);
    private readonly AudioClip _fartZombieAudioClip;
    
    public ZombieBoozerAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty, LevelAudioClipProvider levelAudioClipProvider)
        : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _fartZombieAudioClip = levelAudioClipProvider.FartZombieAudioClip;
    }
    public void PlayFart()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_fartZombieAudioClip, _volumeSection, _pitchSection);
    }
}