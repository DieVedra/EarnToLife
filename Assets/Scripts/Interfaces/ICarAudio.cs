using System;
using UniRx;
using UnityEngine;

public interface ICarAudio
{
    public CarAudioClipProvider CarAudioClipProvider { get; }
    public AudioSource CarAudioSourceForEngine { get; }
    public AudioSource CarAudioSourceForBooster { get; }
    public AudioSource CarAudioSourceForDestruction { get; }
    public AudioSource CarAudioSourceForHotWheels { get; }
    public AudioSource CarAudioSourceForOther { get; }
    public ReactiveProperty<bool> SoundReactiveProperty { get; }
    public event Action OnSoundChange; 
}