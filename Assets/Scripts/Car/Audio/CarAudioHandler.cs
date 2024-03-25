using System;
public class CarAudioHandler : IDisposable
{
    public readonly EngineAudioHandler EngineAudioHandler;
    public readonly BoosterAudioHandler BoosterAudioHandler;
    public readonly BrakeAudioHandler BrakeAudioHandler;
    public readonly GunAudioHandler GunAudioHandler;
    private readonly ICarAudio _globalAudioToCar;
    
    public CarAudioHandler(ICarAudio globalAudioToCar)
    {
        _globalAudioToCar = globalAudioToCar;
        EngineAudioHandler = new EngineAudioHandler(_globalAudioToCar.CarAudioSource1, _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarClips.EngineStartAudioClip,
            _globalAudioToCar.CarClips.EngineStopAudioClip,
            _globalAudioToCar.CarClips.EngineRunAudioClip);
        BoosterAudioHandler = new BoosterAudioHandler(_globalAudioToCar.CarAudioSource2, _globalAudioToCar.SoundReactiveProperty,
            _globalAudioToCar.CarClips.BoosterStartAudioClip,
            _globalAudioToCar.CarClips.BoosterStopAudioClip,
            _globalAudioToCar.CarClips.BoosterRunAudioClip);
        BrakeAudioHandler = new BrakeAudioHandler(_globalAudioToCar.CarAudioSource2, _globalAudioToCar.SoundReactiveProperty, _globalAudioToCar.CarClips.BrakeAudioClip);
        GunAudioHandler = new GunAudioHandler(_globalAudioToCar.CarAudioSource1, _globalAudioToCar.SoundReactiveProperty, _globalAudioToCar.CarClips.ShotGunAudioClip);
        _globalAudioToCar.OnSoundChange += EngineAudioHandler.PlayRun;
    }
    public void Dispose()
    {
        _globalAudioToCar.OnSoundChange -= EngineAudioHandler.PlayRun;
        _globalAudioToCar.SoundReactiveProperty.Dispose();
    }
}
