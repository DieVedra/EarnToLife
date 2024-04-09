using System;
public class LevelAudioHandler : IDisposable
{
    public readonly WoodDestructibleAudioHandler WoodDestructibleAudioHandler;
    public readonly BarrelAudioHandler BarrelAudioHandler;
    private readonly ILevelAudio _globalAudioToCar;
    
    public LevelAudioHandler(ILevelAudio globalAudioToCar)
    {
        _globalAudioToCar = globalAudioToCar;
        WoodDestructibleAudioHandler = new WoodDestructibleAudioHandler(globalAudioToCar.LevelAudioSource, globalAudioToCar.SoundReactiveProperty,
            globalAudioToCar.LevelAudioClipProvider.WoodBreaking1AudioClip,
            globalAudioToCar.LevelAudioClipProvider.WoodBreaking2AudioClip,
            globalAudioToCar.LevelAudioClipProvider.HitWood1AudioClip,
            globalAudioToCar.LevelAudioClipProvider.HitWood2AudioClip,
            globalAudioToCar.LevelAudioClipProvider.HitWood3AudioClip);
        BarrelAudioHandler = new BarrelAudioHandler(globalAudioToCar.LevelAudioSource, globalAudioToCar.SoundReactiveProperty,
            globalAudioToCar.LevelAudioClipProvider.HitBarrelAudioClip,
            globalAudioToCar.LevelAudioClipProvider.HitDebrisBarrelAudioClip,
            globalAudioToCar.LevelAudioClipProvider.ExplodeBarrelAudioClip);
    }
    public void Dispose()
    {
        _globalAudioToCar.SoundReactiveProperty.Dispose();
    }
}
