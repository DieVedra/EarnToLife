using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CarDefaultAudioClipProvider", menuName = "Providers/CarDefaultAudioClipProvider", order = 51)]
public class CarDefaultAudioClipProvider : ScriptableObject
{
    [HorizontalLine( color:EColor.White)]
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _brakeAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _engineStopAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _carBurnAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _carHardHitAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _carSoftHitAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _glassBreakingAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _metalBendsAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _driverNeckBrokeAudioClip;
    [SerializeField, BoxGroup("DefaultSoundsCar")] private AudioClip _frictionAudioClip;
    
    public AudioClip BrakeAudioClip => _brakeAudioClip;
    public AudioClip EngineStopAudioClip => _engineStopAudioClip;
    public AudioClip CarBurnAudioClip => _carBurnAudioClip;
    public AudioClip CarHardHitAudioClip => _carHardHitAudioClip;
    public AudioClip CarSoftHitAudioClip => _carSoftHitAudioClip;
    public AudioClip GlassBreakingAudioClip => _glassBreakingAudioClip;
    public AudioClip MetalBendsAudioClip => _metalBendsAudioClip; 
    public AudioClip DriverNeckBrokeAudioClip => _driverNeckBrokeAudioClip;
    public AudioClip FrictionAudioClip => _frictionAudioClip;
}
