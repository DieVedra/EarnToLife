using System;
public class CarAudioHandler : IDisposable
{
    public readonly EngineAudioHandler EngineAudioHandler;
    public readonly BoosterAudioHandler BoosterAudioHandler;
    public readonly BrakeAudioHandler BrakeAudioHandler;
    public readonly GunAudioHandler GunAudioHandler;
    public readonly DestructionAudioHandler DestructionAudioHandler;
    public readonly HotWheelAudioHandler HotWheelAudioHandler;
    private readonly ICarAudio _globalAudioToCar;
    
    public CarAudioHandler(ICarAudio globalAudioToCar)
    {
        _globalAudioToCar = globalAudioToCar;
        EngineAudioHandler = new EngineAudioHandler(_globalAudioToCar.CarAudioSourceForEngine,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarAudioClipProvider.EngineStartAudioClip,
            _globalAudioToCar.CarAudioClipProvider.EngineStopAudioClip,
            _globalAudioToCar.CarAudioClipProvider.EngineRunAudioClip);
        BoosterAudioHandler = new BoosterAudioHandler(_globalAudioToCar.CarAudioSourceForBooster,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarAudioClipProvider.BoosterRunAudioClip);
        BrakeAudioHandler = new BrakeAudioHandler(_globalAudioToCar.CarAudioSourceForOther,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarAudioClipProvider.BrakeAudioClip);
        GunAudioHandler = new GunAudioHandler(_globalAudioToCar.CarAudioSourceForOther,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarAudioClipProvider.ShotGunAudioClip);
        DestructionAudioHandler = new DestructionAudioHandler(_globalAudioToCar.CarAudioSourceForDestruction, _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarAudioClipProvider.CarBurnAudioClip,
            _globalAudioToCar.CarAudioClipProvider.CarHardHitAudioClip,
            _globalAudioToCar.CarAudioClipProvider.CarSoftHitAudioClip,
            _globalAudioToCar.CarAudioClipProvider.GlassBreakingAudioClip,
            _globalAudioToCar.CarAudioClipProvider.MetalBendsAudioClip,
            _globalAudioToCar.CarAudioClipProvider.EngineClapAudioClip,
            _globalAudioToCar.CarAudioClipProvider.DriverNeckBrokeAudioClip
        );
        HotWheelAudioHandler = new HotWheelAudioHandler(_globalAudioToCar.CarAudioSourceForHotWheels, _globalAudioToCar.CarAudioSourceForOther, _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarAudioClipProvider.CarHotweelAudioClip,
            _globalAudioToCar.CarAudioClipProvider.CarHotweelSlitAudioClip);
        _globalAudioToCar.OnSoundChange += EngineAudioHandler.PlayRun;
    }
    public void Dispose()
    {
        _globalAudioToCar.OnSoundChange -= EngineAudioHandler.PlayRun;
        _globalAudioToCar.SoundReactiveProperty.Dispose();
        HotWheelAudioHandler.Dispose();
    }
}
