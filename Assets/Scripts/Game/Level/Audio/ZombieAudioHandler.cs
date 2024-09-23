using UniRx;
using UnityEngine;

public class ZombieAudioHandler : AudioPlayer
{
    private readonly Vector2 _pitchSection = new Vector2(0.9f,1.1f);
    private readonly Vector2 _volumeSection = new Vector2(0.7f,1f);
    
    private readonly AudioClip[] _deathClips;
    private readonly AudioClip[] _talkClips;
    private readonly AudioClip[] _hitClips;

    public ZombieAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
    LevelAudioClipProvider levelAudioClipProvider)
        :base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _deathClips = new[]
        {
            levelAudioClipProvider.Death1ZombieAudioClip,
            levelAudioClipProvider.Death2ZombieAudioClip,
            levelAudioClipProvider.Death3ZombieAudioClip,
            levelAudioClipProvider.Death4ZombieAudioClip,
            levelAudioClipProvider.Death5ZombieAudioClip
        };
        _talkClips = new[]
        {
            levelAudioClipProvider.Talk1ZombieAudioClip,
            levelAudioClipProvider.Talk2ZombieAudioClip,
            levelAudioClipProvider.Talk3ZombieAudioClip,
            levelAudioClipProvider.Talk4ZombieAudioClip,
            levelAudioClipProvider.Talk5ZombieAudioClip,
            levelAudioClipProvider.Talk6ZombieAudioClip,
        };
        _hitClips = new[]
        {
            levelAudioClipProvider.Hit1ZombieAudioClip,
            levelAudioClipProvider.Hit2ZombieAudioClip
        };
    }
    public void PlayDeath()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(_deathClips), GetRandomFloatValue(_volumeSection.x, _volumeSection.y), _pitchSection);
    }

    public void PlayTalk()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(_talkClips), GetRandomFloatValue(_volumeSection.x, _volumeSection.y), _pitchSection);
    }

    public void PlayHit(float volume)
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(_hitClips), volume, _pitchSection);
    }
}