using System;
using UniRx;
using UnityEngine;

public class LevelAudioHandler : IDisposable
{
    public readonly WoodDestructibleAudioHandler WoodDestructibleAudioHandler;
    public readonly BarrelAudioHandler BarrelAudioHandler;
    private readonly ReactiveProperty<bool> _soundReactiveProperty;
    public LevelAudioHandler(AudioSource levelAudioSource, IGlobalAudio globalAudio, LevelAudioClipProvider levelAudioClipProvider)
    {
        _soundReactiveProperty = globalAudio.SoundReactiveProperty;
        WoodDestructibleAudioHandler = new WoodDestructibleAudioHandler(levelAudioSource, globalAudio.SoundReactiveProperty,
            levelAudioClipProvider.WoodBreaking1AudioClip,
            levelAudioClipProvider.WoodBreaking2AudioClip,
            levelAudioClipProvider.HitWood1AudioClip,
            levelAudioClipProvider.HitWood2AudioClip,
            levelAudioClipProvider.HitWood3AudioClip);
        BarrelAudioHandler = new BarrelAudioHandler(levelAudioSource, globalAudio.SoundReactiveProperty,
            levelAudioClipProvider.HitBarrelAudioClip,
            levelAudioClipProvider.HitDebrisBarrelAudioClip,
            levelAudioClipProvider.ExplodeBarrelAudioClip);
    }
    public void Dispose()
    {
        _soundReactiveProperty.Dispose();
    }
}
