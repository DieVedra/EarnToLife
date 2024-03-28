using System;
public class CarAudioHandler : IDisposable
{
    public readonly EngineAudioHandler EngineAudioHandler;
    public readonly BoosterAudioHandler BoosterAudioHandler;
    public readonly BrakeAudioHandler BrakeAudioHandler;
    public readonly GunAudioHandler GunAudioHandler;
    public readonly DestructionAudioHandler DestructionAudioHandler;
    private readonly ICarAudio _globalAudioToCar;
    
    public CarAudioHandler(ICarAudio globalAudioToCar)
    {
        _globalAudioToCar = globalAudioToCar;
        EngineAudioHandler = new EngineAudioHandler(_globalAudioToCar.CarAudioSourceForEngine,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarClips.EngineStartAudioClip,
            _globalAudioToCar.CarClips.EngineStopAudioClip,
            _globalAudioToCar.CarClips.EngineRunAudioClip);
        BoosterAudioHandler = new BoosterAudioHandler(_globalAudioToCar.CarAudioSourceForBooster,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarClips.BoosterRunAudioClip);
        BrakeAudioHandler = new BrakeAudioHandler(_globalAudioToCar.CarAudioSourceForOther,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarClips.BrakeAudioClip);
        GunAudioHandler = new GunAudioHandler(_globalAudioToCar.CarAudioSourceForOther,
            _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarClips.ShotGunAudioClip);
        DestructionAudioHandler = new DestructionAudioHandler(_globalAudioToCar.CarAudioSourceForDestruction, _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarClips.CarBurnAudioClip,
            _globalAudioToCar.CarClips.CarHardHitAudioClip,
            _globalAudioToCar.CarClips.CarSoftHitAudioClip,
            _globalAudioToCar.CarClips.GlassBreakingAudioClip,
            _globalAudioToCar.CarClips.MetalBendsAudioClip);
        _globalAudioToCar.OnSoundChange += EngineAudioHandler.PlayRun;
    }
    public void Dispose()
    {
        _globalAudioToCar.OnSoundChange -= EngineAudioHandler.PlayRun;
        _globalAudioToCar.SoundReactiveProperty.Dispose();
    }
}
