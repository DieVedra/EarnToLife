using UnityEngine;

public class CarClips
{
    public AudioClip EngineRunAudioClip { get; }
     public AudioClip BrakeAudioClip { get; }
     public AudioClip EngineStartAudioClip { get; }
     public AudioClip EngineStopAudioClip { get; }
     public AudioClip BoosterRunAudioClip { get; }
     public AudioClip ShotGunAudioClip { get; }
     public AudioClip CarHotweelAudioClip { get; }
     public AudioClip CarBurnAudioClip { get; }
     public AudioClip CarHardHitAudioClip { get; }
     public AudioClip CarSoftHitAudioClip { get; }
     public AudioClip GlassBreakingAudioClip { get; }
     public AudioClip MetalBendsAudioClip { get; }
     public AudioClip EngineClapAudioClip { get; }
     public AudioClip DriverNeckBrokeAudioClip { get; }
     public CarClips(AudioClip engineRunAudioClip, AudioClip brakeAudioClip, AudioClip engineStartAudioClip,
         AudioClip engineStopAudioClip, AudioClip boosterRunAudioClip, AudioClip shotGunAudioClip, AudioClip carHotweelAudioClip,
         AudioClip carBurnAudioClip, AudioClip carHardHitAudioClip, AudioClip carSoftHitAudioClip,AudioClip glassBreakingAudioClip,
         AudioClip metalBendsAudioClip, AudioClip engineClapAudioClip, AudioClip driverNeckBrokeAudioClip)
     {
         EngineRunAudioClip = engineRunAudioClip;
         BrakeAudioClip = brakeAudioClip;
         EngineStartAudioClip = engineStartAudioClip;
         EngineStopAudioClip = engineStopAudioClip;
         BoosterRunAudioClip = boosterRunAudioClip;
         ShotGunAudioClip = shotGunAudioClip;
         CarHotweelAudioClip = carHotweelAudioClip;
         CarBurnAudioClip = carBurnAudioClip;
         CarHardHitAudioClip = carHardHitAudioClip;
         CarSoftHitAudioClip = carSoftHitAudioClip;
         GlassBreakingAudioClip = glassBreakingAudioClip;
         MetalBendsAudioClip = metalBendsAudioClip;
         EngineClapAudioClip = engineClapAudioClip;
         DriverNeckBrokeAudioClip = driverNeckBrokeAudioClip;
     }
}