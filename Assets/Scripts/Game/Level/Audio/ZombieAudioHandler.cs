
using UniRx;
using UnityEngine;

public class ZombieAudioHandler: AudioPlayer
{
    private readonly Vector2 _pitchSection = new Vector2(0.9f,1.1f);
    private readonly Vector2 _volumeSection = new Vector2(0.7f,1f);
    private readonly AudioClip _hit1Zombie;
    private readonly AudioClip _hit2Zombie;
    private readonly AudioClip _death1ZombieAudioClip;
    private readonly AudioClip _death2ZombieAudioClip;
    private readonly AudioClip _death3ZombieAudioClip;
    private readonly AudioClip _death4ZombieAudioClip;
    private readonly AudioClip _death5ZombieAudioClip;
    private readonly AudioClip _talk1ZombieAudioClip;
    private readonly AudioClip _talk2ZombieAudioClip;
    private readonly AudioClip _talk3ZombieAudioClip;
    private readonly AudioClip _talk4ZombieAudioClip;
    private readonly AudioClip _talk5ZombieAudioClip;
    private readonly AudioClip _talk6ZombieAudioClip;
    private readonly AudioClip _fartZombieAudioClip;
    private readonly AudioClip[] _deathClips;
    private readonly AudioClip[] _talkClips;

public ZombieAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip hit1Zombie, AudioClip hit2Zombie,
        AudioClip death1ZombieAudioClip, AudioClip death2ZombieAudioClip, AudioClip death3ZombieAudioClip, AudioClip death4ZombieAudioClip,
        AudioClip death5ZombieAudioClip,
        AudioClip talk1ZombieAudioClip, AudioClip talk2ZombieAudioClip, AudioClip talk3ZombieAudioClip, AudioClip talk4ZombieAudioClip,
        AudioClip talk5ZombieAudioClip, AudioClip talk6ZombieAudioClip,
        AudioClip fartZombieAudioClip)
        :base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _hit1Zombie = hit1Zombie;
        _hit2Zombie = hit2Zombie;
        _death1ZombieAudioClip = death1ZombieAudioClip;
        _death2ZombieAudioClip = death2ZombieAudioClip;
        _death3ZombieAudioClip = death3ZombieAudioClip;
        _death4ZombieAudioClip = death4ZombieAudioClip;
        _death5ZombieAudioClip = death5ZombieAudioClip;
        _talk1ZombieAudioClip = talk1ZombieAudioClip;
        _talk2ZombieAudioClip = talk2ZombieAudioClip;
        _talk3ZombieAudioClip = talk3ZombieAudioClip;
        _talk4ZombieAudioClip = talk4ZombieAudioClip;
        _talk5ZombieAudioClip = talk5ZombieAudioClip;
        _talk6ZombieAudioClip = talk6ZombieAudioClip;
        _fartZombieAudioClip = fartZombieAudioClip;
        _deathClips = new[]
        {
            death1ZombieAudioClip,
            death2ZombieAudioClip,
            death3ZombieAudioClip,
            death4ZombieAudioClip,
            death5ZombieAudioClip
        };
        _talkClips = new[]
        {
            talk1ZombieAudioClip,
            talk2ZombieAudioClip,
            talk3ZombieAudioClip,
            talk4ZombieAudioClip,
            talk5ZombieAudioClip,
            talk6ZombieAudioClip
        };
    }

    public void PlayFart()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(_fartZombieAudioClip, _volumeSection, _pitchSection);
    }

    public void PlayDeath()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(_deathClips), _volumeSection, _pitchSection);
    }

    public void PlayTalk()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(_talkClips), _volumeSection, _pitchSection);
    }
}