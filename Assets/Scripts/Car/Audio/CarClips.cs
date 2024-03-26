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
     public AudioClip CarHitAudioClip { get; }
     public AudioClip GlassBreakingAudioClip { get; }
     public CarClips(AudioClip engineRunAudioClip, AudioClip brakeAudioClip, AudioClip engineStartAudioClip,
         AudioClip engineStopAudioClip, AudioClip boosterRunAudioClip, AudioClip shotGunAudioClip, AudioClip carHotweelAudioClip,
         AudioClip carBurnAudioClip, AudioClip carHitAudioClip, AudioClip glassBreakingAudioClip)
     {
         EngineRunAudioClip = engineRunAudioClip;
         BrakeAudioClip = brakeAudioClip;
         EngineStartAudioClip = engineStartAudioClip;
         EngineStopAudioClip = engineStopAudioClip;
         BoosterRunAudioClip = boosterRunAudioClip;
         ShotGunAudioClip = shotGunAudioClip;
     }
}
