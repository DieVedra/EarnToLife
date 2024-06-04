using System;
using UnityEngine;

public class LevelAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _otherAudioSource;
    [SerializeField] private AudioSource _zombieAudioSource;
    [SerializeField] private AudioSource _debrisAudioSource;
    private LevelAudioClipProvider _levelAudioClipProvider;
    public WoodDestructibleAudioHandler WoodDestructibleAudioHandler { get; private set; }
    public BarrelAudioHandler BarrelAudioHandler { get; private set; }
    public ZombieAudioHandler ZombieAudioHandler { get; private set; }
    public DebrisAudioHandler DebrisAudioHandler { get; private set; }


    public void Init(AudioClipProvider audioClipProvider, IGlobalAudio globalAudio)
    {
        _levelAudioClipProvider = audioClipProvider.LevelAudioClipProvider;
        WoodDestructibleAudioHandler = new WoodDestructibleAudioHandler(_otherAudioSource,
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty,
            _levelAudioClipProvider.WoodBreaking1AudioClip,
            _levelAudioClipProvider.WoodBreaking2AudioClip,
            _levelAudioClipProvider.HitWood1AudioClip,
            _levelAudioClipProvider.HitWood2AudioClip,
            _levelAudioClipProvider.HitWood3AudioClip);
        
        BarrelAudioHandler = new BarrelAudioHandler(_otherAudioSource,
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty,
            _levelAudioClipProvider.HitBarrelAudioClip,
            _levelAudioClipProvider.Explode1BarrelAudioClip,
            _levelAudioClipProvider.Explode2BarrelAudioClip,
            _levelAudioClipProvider.BurnBarrelAudioClip);
        
        ZombieAudioHandler = new ZombieAudioHandler(_zombieAudioSource,
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty,
            _levelAudioClipProvider.Hit1ZombieAudioClip,
            _levelAudioClipProvider.Hit2ZombieAudioClip,
            _levelAudioClipProvider.Death1ZombieAudioClip,
            _levelAudioClipProvider.Death2ZombieAudioClip,
            _levelAudioClipProvider.Death3ZombieAudioClip,
            _levelAudioClipProvider.Death4ZombieAudioClip,
            _levelAudioClipProvider.Death5ZombieAudioClip,
            _levelAudioClipProvider.Talk1ZombieAudioClip,
            _levelAudioClipProvider.Talk2ZombieAudioClip,
            _levelAudioClipProvider.Talk3ZombieAudioClip,
            _levelAudioClipProvider.Talk4ZombieAudioClip,
            _levelAudioClipProvider.Talk5ZombieAudioClip,
            _levelAudioClipProvider.Talk6ZombieAudioClip,
            _levelAudioClipProvider.FartZombieAudioClip);
        
        DebrisAudioHandler = new DebrisAudioHandler(_debrisAudioSource, globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty,
            _levelAudioClipProvider.Hit1DebrisBarrelAudioClip,
            _levelAudioClipProvider.Hit2DebrisBarrelAudioClip,
            _levelAudioClipProvider.BurnBarrelAudioClip
            );
    }
}