using UniRx;
using UnityEngine;

public class TrapAudioHandler : AudioPlayer
{
    private readonly AudioClip _collapsing;
    private readonly AudioClip _impactOfTheFall;

    public TrapAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty, LevelAudioClipProvider levelAudioClipProvider)
    :base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _collapsing = levelAudioClipProvider.Collapsing;
        _impactOfTheFall = levelAudioClipProvider.ImpactOfTheFall;
    }

    public void PlayFall()
    {
        TryPlayOneShotClip(_collapsing);
    }
    public void PlayHit()
    {
        TryPlayOneShotClip(_impactOfTheFall);
    }
}