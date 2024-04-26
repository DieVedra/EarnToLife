using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CarAudioClipProvider", menuName = "Providers/CarAudioClipProvider", order = 51)]
public class CarAudioClipProvider : ScriptableObject
{
    [HorizontalLine( color:EColor.Black)]
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineRunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineStartAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _boosterRunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _boosterEndFuelAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carHotweelAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carHotweelSlitAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _shotGunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineBrokenAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _suspensionAudioClip;

    public AudioClip EngineRunAudioClip => _engineRunAudioClip;
    public AudioClip EngineStartAudioClip => _engineStartAudioClip;
    public AudioClip BoosterRunAudioClip => _boosterRunAudioClip;
    public AudioClip BoosterEndFuelAudioClip => _boosterEndFuelAudioClip;
    public AudioClip CarHotweelAudioClip => _carHotweelAudioClip;
    public AudioClip CarHotweelSlitAudioClip => _carHotweelSlitAudioClip;
    public AudioClip ShotGunAudioClip => _shotGunAudioClip;
    public AudioClip EngineBrokenAudioClip => _engineBrokenAudioClip;
    public AudioClip SuspensionAudioClip => _suspensionAudioClip;
    public CarDefaultAudioClipProvider CarDefaultAudioClipProvider { get; private set; }

    public void Init(CarDefaultAudioClipProvider carDefaultAudioClipProvider)
    {
        CarDefaultAudioClipProvider = carDefaultAudioClipProvider;
    }
}
