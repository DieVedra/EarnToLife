using NaughtyAttributes;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    [SerializeField, BoxGroup("WheelGroundInteraction")] private AnimationCurve _brakeVolumeCurve;
    [SerializeField, BoxGroup("Booster")] private AnimationCurve _increaseBoosterSoundCurve;
    [SerializeField, BoxGroup("Booster")] private AnimationCurve _decreaseBoosterSoundCurve;
    [SerializeField, BoxGroup("Suspension")] private AnimationCurve _suspensionSoundCurve;

    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forEngine;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forBooster;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forDestruction;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forHotWheels1;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forHotWheels2;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forBrakes;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _frontSuspension;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _backSuspension;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _frictionAudioSource;

    private CarAudioInitializer _carAudioInitializer;

    public EngineAudioHandler EngineAudioHandler => _carAudioInitializer.EngineAudioHandler;

    public BoosterAudioHandler BoosterAudioHandler => _carAudioInitializer.BoosterAudioHandler;

    public BrakeAudioHandler BrakeAudioHandler => _carAudioInitializer.BrakeAudioHandler;

    public GunAudioHandler GunAudioHandler => _carAudioInitializer.GunAudioHandler;

    public DestructionAudioHandler DestructionAudioHandler => _carAudioInitializer.DestructionAudioHandler;

    public HotWheelAudioHandler HotWheelAudioHandler => _carAudioInitializer.HotWheelAudioHandler;

    public SuspensionAudioHandler SuspensionAudioHandler => _carAudioInitializer.SuspensionAudioHandler;


    public void Construct(IGlobalAudio globalAudio, CarAudioClipProvider carAudioClipProvider)
    {
        _carAudioInitializer = new CarAudioInitializer(globalAudio, carAudioClipProvider,
            _forEngine, _forBooster, _forDestruction, _forHotWheels1, _forHotWheels2,
            _forBrakes, _frontSuspension, _backSuspension, _frictionAudioSource,
            
            _increaseBoosterSoundCurve, _decreaseBoosterSoundCurve, _suspensionSoundCurve, _brakeVolumeCurve);
    }
}