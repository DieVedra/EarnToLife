using System;
using UnityEngine;

public class CarAudioInitializer
{
    public readonly EngineAudioHandler EngineAudioHandler;
    public readonly BoosterAudioHandler BoosterAudioHandler;
    public readonly BrakeAudioHandler BrakeAudioHandler;
    public readonly GunAudioHandler GunAudioHandler;
    public readonly DestructionAudioHandler DestructionAudioHandler;
    public readonly HotWheelAudioHandler HotWheelAudioHandler;
    public readonly SuspensionAudioHandler SuspensionAudioHandler;
    private readonly IGlobalAudio _globalAudio;
    
    public CarAudioInitializer(IGlobalAudio globalAudio, CarAudioClipProvider carAudioClipProvider,
        AudioSource forEngine, AudioSource forBooster, AudioSource forDestruction, AudioSource forHotWheels1,
            AudioSource forHotWheels2, AudioSource forBrakes, AudioSource frontSuspension, AudioSource backSuspension, AudioSource frictionAudioSource,
        
        
        AnimationCurve increaseBoosterSoundCurve, AnimationCurve decreaseBoosterSoundCurve, 
        AnimationCurve suspensionSoundCurve, AnimationCurve brakeVolumeCurve)
    {
        _globalAudio = globalAudio;
        EngineAudioHandler = new EngineAudioHandler(forEngine,
            _globalAudio.SoundReactiveProperty,
            carAudioClipProvider.EngineStartAudioClip,
            carAudioClipProvider.CarDefaultAudioClipProvider.EngineStopAudioClip,
            carAudioClipProvider.EngineRunAudioClip);
        BoosterAudioHandler = new BoosterAudioHandler(forBooster,
            _globalAudio.SoundReactiveProperty,
            carAudioClipProvider.BoosterRunAudioClip,
            carAudioClipProvider.BoosterEndFuelAudioClip,
            increaseBoosterSoundCurve, decreaseBoosterSoundCurve);
        BrakeAudioHandler = new BrakeAudioHandler(forBrakes,
            _globalAudio.SoundReactiveProperty,
            carAudioClipProvider.CarDefaultAudioClipProvider.BrakeAudioClip,
            brakeVolumeCurve);
        GunAudioHandler = new GunAudioHandler(forBrakes,
            _globalAudio.SoundReactiveProperty,
            carAudioClipProvider.ShotGunAudioClip);
        DestructionAudioHandler = new DestructionAudioHandler(forDestruction, _globalAudio.SoundReactiveProperty,
            carAudioClipProvider.CarDefaultAudioClipProvider.CarBurnAudioClip,
            carAudioClipProvider.CarDefaultAudioClipProvider.CarHardHitAudioClip,
            carAudioClipProvider.CarDefaultAudioClipProvider.CarSoftHitAudioClip,
            carAudioClipProvider.CarDefaultAudioClipProvider.GlassBreakingAudioClip,
            carAudioClipProvider.CarDefaultAudioClipProvider.MetalBendsAudioClip,
            carAudioClipProvider.EngineBrokenAudioClip,
            carAudioClipProvider.CarDefaultAudioClipProvider.DriverNeckBrokeAudioClip
        );
        HotWheelAudioHandler = new HotWheelAudioHandler(
            forHotWheels1, forHotWheels2,
            _globalAudio.SoundReactiveProperty,
            carAudioClipProvider.CarHotweelAudioClip,
            carAudioClipProvider.CarHotweelSlitAudioClip);
        SuspensionAudioHandler = new SuspensionAudioHandler(
            frontSuspension, backSuspension,
            _globalAudio.SoundReactiveProperty,
            carAudioClipProvider.SuspensionAudioClip,
            carAudioClipProvider.CarDefaultAudioClipProvider.FrictionAudioClip,
            suspensionSoundCurve);
        
        
        
        // _globalAudio.OnSoundChange += EngineAudioHandler.PlayRun;
    }
    public void Dispose()
    {
        // _globalAudio.OnSoundChange -= EngineAudioHandler.PlayRun;
        EngineAudioHandler.Dispose();
        _globalAudio.SoundReactiveProperty.Dispose();
        HotWheelAudioHandler.Dispose();
        SuspensionAudioHandler.Dispose();
    }
}
