using System;
using UnityEngine;

public class LevelAudioHandler
{
    public readonly WoodDestructibleAudioHandler WoodDestructibleAudioHandler;
    public readonly BarrelAudioHandler BarrelAudioHandler;
    public LevelAudioHandler(AudioSource levelAudioSource, IGlobalAudio globalAudio, LevelAudioClipProvider levelAudioClipProvider)
    {
        WoodDestructibleAudioHandler = new WoodDestructibleAudioHandler(levelAudioSource,
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty,
            levelAudioClipProvider.WoodBreaking1AudioClip,
            levelAudioClipProvider.WoodBreaking2AudioClip,
            levelAudioClipProvider.HitWood1AudioClip,
            levelAudioClipProvider.HitWood2AudioClip,
            levelAudioClipProvider.HitWood3AudioClip);
        BarrelAudioHandler = new BarrelAudioHandler(levelAudioSource,
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty,
            levelAudioClipProvider.HitBarrelAudioClip,
            levelAudioClipProvider.Hit1DebrisBarrelAudioClip,
            levelAudioClipProvider.Hit2DebrisBarrelAudioClip,
            levelAudioClipProvider.Explode1BarrelAudioClip,
            levelAudioClipProvider.Explode2BarrelAudioClip);
    }
}
