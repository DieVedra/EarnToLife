using UnityEngine;

public class CarClips
{
    public AudioClip EngineRunAudioClip { get; }
     public AudioClip BrakeAudioClip { get; }
     public AudioClip EngineStartAudioClip { get; }
     public AudioClip EngineStopAudioClip { get; }
     public AudioClip BoosterRunAudioClip { get; }
     public AudioClip BoosterStopAudioClip { get; }
     public AudioClip ShotGunAudioClip { get; }
     public CarClips(AudioClip engineRunAudioClip, AudioClip brakeAudioClip, AudioClip engineStartAudioClip,
         AudioClip engineStopAudioClip, AudioClip boosterRunAudioClip, AudioClip boosterStopAudioClip,
         AudioClip shotGunAudioClip)
     {
         EngineRunAudioClip = engineRunAudioClip;
         BrakeAudioClip = brakeAudioClip;
         EngineStartAudioClip = engineStartAudioClip;
         EngineStopAudioClip = engineStopAudioClip;
         BoosterRunAudioClip = boosterRunAudioClip;
         BoosterStopAudioClip = boosterStopAudioClip;
         ShotGunAudioClip = shotGunAudioClip;
     }
}
