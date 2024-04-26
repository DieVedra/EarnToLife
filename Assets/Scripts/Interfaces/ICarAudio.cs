using System;
using UniRx;
using UnityEngine;

public interface ICarAudio
{
    public CarsAudioClipsProvider CarsAudioClipsProvider { get; }
    public AudioSource CarAudioSourceForEngine { get; }
    public AudioSource CarAudioSourceForBooster { get; }
    public AudioSource CarAudioSourceForDestruction { get; }
    public AudioSource CarAudioSourceForHotWheels1 { get; }
    public AudioSource CarAudioSourceForHotWheels2 { get; }
    public AudioSource CarAudioSourceForBrakes { get; }
    public AudioSource CarAudioSourceFrontSuspension { get; }
    public AudioSource CarAudioSourceBackSuspension { get; }
    public AudioSource FrictionAudioSource { get; }
    public ReactiveProperty<bool> SoundReactiveProperty { get; }
    public event Action OnSoundChange; 
}