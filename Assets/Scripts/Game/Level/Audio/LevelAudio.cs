using System;
using UnityEngine;

public class LevelAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _otherAudioSource;
    [SerializeField] private AudioSource _debrisAudioSource;
    private LevelAudioClipProvider _levelAudioClipProvider;
    public WoodDestructibleAudioHandler WoodDestructibleAudioHandler { get; private set; }
    public BarrelAudioHandler BarrelAudioHandler { get; private set; }
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
        DebrisAudioHandler = new DebrisAudioHandler(_debrisAudioSource, globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty,
            _levelAudioClipProvider.Hit1DebrisBarrelAudioClip,
            _levelAudioClipProvider.Hit2DebrisBarrelAudioClip,
            _levelAudioClipProvider.BurnBarrelAudioClip
            );
    }
}