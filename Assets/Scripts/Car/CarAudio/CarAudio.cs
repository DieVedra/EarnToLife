using NaughtyAttributes;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    [SerializeField, BoxGroup("WheelGroundInteraction")] private AnimationCurve _brakeVolumeCurve;
    [SerializeField, BoxGroup("Booster")] private AnimationCurve _increaseBoosterSoundCurve;
    [SerializeField, BoxGroup("Booster")] private AnimationCurve _decreaseBoosterSoundCurve;

    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forEngine;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forBooster;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forGun;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forDestruction;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forHotWheels1;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forHotWheels2;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forWheelsFriction;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forWheelsHit;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _frontSuspension;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _backSuspension;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _frictionAudioSource;

    private IGlobalAudio _globalAudio;
    private CarAudioClipProvider _carAudioClipProvider;
    public EngineAudioHandler EngineAudioHandler { get; private set; }
    public BoosterAudioHandler BoosterAudioHandler { get; private set; }
    public BrakeAudioHandler BrakeAudioHandler => WheelsAudioHandler.BrakeAudioHandler;
    public WheelsAudioHandler WheelsAudioHandler { get; private set; }
    public GunAudioHandler GunAudioHandler { get; private set; }
    public DestructionAudioHandler DestructionAudioHandler { get; private set; }
    public HotWheelAudioHandler HotWheelAudioHandler { get; private set; }
    public SuspensionAudioHandler SuspensionAudioHandler { get; private set; }
    public void Construct(IGlobalAudio globalAudio, CarAudioClipProvider carAudioClipProvider)
    {
        _globalAudio = globalAudio;
        _carAudioClipProvider = carAudioClipProvider;
        InitEngineAudio();
        InitBoosterAudio();
        InitGunAudio();
        InitDestructionAudio();
        InitHotWheelAudio();
        InitSuspensionAudio();
        InitWheelsAudio();
    }
    public void Dispose()
    {
        EngineAudioHandler.Dispose();
        DestructionAudioHandler.Dispose();
        HotWheelAudioHandler.Dispose();
        WheelsAudioHandler.Dispose();
    }

    private void InitEngineAudio()
    {
        EngineAudioHandler = new EngineAudioHandler(_forEngine,
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty,
            _carAudioClipProvider.EngineStartAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.EngineStopAudioClip,
            _carAudioClipProvider.EngineRunAudioClip);
    }

    private void InitBoosterAudio()
    {
        BoosterAudioHandler = new BoosterAudioHandler(_forBooster,
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty,
            _carAudioClipProvider.BoosterRunAudioClip,
            _carAudioClipProvider.BoosterEndFuelAudioClip,
            _increaseBoosterSoundCurve, _decreaseBoosterSoundCurve);
    }

    private void InitGunAudio()
    {
        GunAudioHandler = new GunAudioHandler(_forGun,
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty,
            _carAudioClipProvider.ShotGunAudioClip);
    }

    private void InitDestructionAudio()
    {
        DestructionAudioHandler = new DestructionAudioHandler(_forDestruction,
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty,
            _carAudioClipProvider.CarDefaultAudioClipProvider.CarBurnAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.CarHardHitAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.CarSoftHitAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.GlassBreakingAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.MetalBendsAudioClip,
            _carAudioClipProvider.EngineBrokenAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.DriverNeckBrokeAudioClip
        );
    }

    private void InitHotWheelAudio()
    {
        HotWheelAudioHandler = new HotWheelAudioHandler(
            _forHotWheels1, _forHotWheels2,
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty,
            _carAudioClipProvider.CarHotweelAudioClip,
            _carAudioClipProvider.CarHotweelSlitAudioClip);
    }

    private void InitSuspensionAudio()
    {
        SuspensionAudioHandler = new SuspensionAudioHandler(
            _frontSuspension, _backSuspension,
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty,
            _carAudioClipProvider.SuspensionAudioClip);
    }

    private void InitWheelsAudio()
    {
        WheelsAudioHandler = new WheelsAudioHandler(_forWheelsFriction, _forWheelsHit,
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty,
            _carAudioClipProvider.CarDefaultAudioClipProvider.BrakeAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.FrictionAudioClip,
            _carAudioClipProvider.CarDefaultAudioClipProvider.WheelHitAudioClip,
            _brakeVolumeCurve);
    }
}