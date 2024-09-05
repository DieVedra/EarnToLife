using System;
using UnityEngine;

public class LevelAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _otherAudioSource;
    [SerializeField] private AudioSource _debrisAudioSource;
    private LevelAudioClipProvider _levelAudioClipProvider;
    // public WoodDestructibleAudioHandler WoodDestructibleAudioHandler { get; private set; }
    // public BarrelAudioHandler BarrelAudioHandler { get; private set; }
    public DebrisAudioHandler DebrisAudioHandler { get; private set; }


    public void Init(AudioClipProvider audioClipProvider, TimeScaleSignal timeScaleSignal, IGlobalAudio globalAudio)
    {
        _levelAudioClipProvider = audioClipProvider.LevelAudioClipProvider;
        DebrisAudioHandler = new DebrisAudioHandler(_debrisAudioSource, globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty, new TimeScalePitchHandler(timeScaleSignal),
            _levelAudioClipProvider.Hit1DebrisBarrelAudioClip,
            _levelAudioClipProvider.Hit2DebrisBarrelAudioClip,
            _levelAudioClipProvider.BurnBarrelAudioClip
            );
    }

    public void Dispose()
    {
        DebrisAudioHandler?.Dispose();
    }
}