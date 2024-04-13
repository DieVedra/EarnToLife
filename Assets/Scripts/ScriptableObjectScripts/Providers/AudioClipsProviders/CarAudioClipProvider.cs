using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CarAudioClipProvider", menuName = "Providers/CarAudioClipProvider", order = 51)]
public class CarAudioClipProvider : ScriptableObject
{
    [SerializeField, BoxGroup("SoundsCar"), HorizontalLine( color:EColor.White)] private AudioClip _engineRunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _brakeAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineStartAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineStopAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _boosterRunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carHotweelAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carHotweelSlitAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carBurnAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carHardHitAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carSoftHitAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _glassBreakingAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _metalBendsAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _shotGunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineClapAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _driverNeckBrokeAudioClip;
    
    public AudioClip EngineRunAudioClip => _engineRunAudioClip;
    public AudioClip BrakeAudioClip => _brakeAudioClip;
    public AudioClip EngineStartAudioClip => _engineStartAudioClip;
    public AudioClip EngineStopAudioClip => _engineStopAudioClip;
    public AudioClip BoosterRunAudioClip => _boosterRunAudioClip;
    public AudioClip ShotGunAudioClip => _shotGunAudioClip;
    public AudioClip CarHotweelAudioClip => _carHotweelAudioClip;
    public AudioClip CarHotweelSlitAudioClip => _carHotweelSlitAudioClip;
    public AudioClip CarBurnAudioClip => _carBurnAudioClip;
    public AudioClip CarHardHitAudioClip => _carHardHitAudioClip;
    public AudioClip CarSoftHitAudioClip => _carSoftHitAudioClip;
    public AudioClip GlassBreakingAudioClip => _glassBreakingAudioClip;
    public AudioClip MetalBendsAudioClip => _metalBendsAudioClip;
    public AudioClip EngineClapAudioClip => _engineClapAudioClip;
    public AudioClip DriverNeckBrokeAudioClip => _driverNeckBrokeAudioClip;
}
