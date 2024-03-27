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
     public AudioClip CarHit1AudioClip { get; }
     public AudioClip CarHit2AudioClip { get; }
     public AudioClip GlassBreakingAudioClip { get; }
     public AudioClip MetalBendsAudioClip { get; }
     public CarClips(AudioClip engineRunAudioClip, AudioClip brakeAudioClip, AudioClip engineStartAudioClip,
         AudioClip engineStopAudioClip, AudioClip boosterRunAudioClip, AudioClip shotGunAudioClip, AudioClip carHotweelAudioClip,
         AudioClip carBurnAudioClip, AudioClip carHit1AudioClip, AudioClip carHit2AudioClip,AudioClip glassBreakingAudioClip, AudioClip metalBendsAudioClip)
     {
         EngineRunAudioClip = engineRunAudioClip;
         BrakeAudioClip = brakeAudioClip;
         EngineStartAudioClip = engineStartAudioClip;
         EngineStopAudioClip = engineStopAudioClip;
         BoosterRunAudioClip = boosterRunAudioClip;
         ShotGunAudioClip = shotGunAudioClip;
         CarHotweelAudioClip = carHotweelAudioClip;
         CarBurnAudioClip = carBurnAudioClip;
         CarHit1AudioClip = carHit1AudioClip;
         CarHit2AudioClip = carHit2AudioClip;
         GlassBreakingAudioClip = glassBreakingAudioClip;
         MetalBendsAudioClip = metalBendsAudioClip;
     }
}
