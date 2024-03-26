using System;
using UniRx;
using UnityEngine;

public interface ICarAudio
{
    public CarClips CarClips { get; }
    public AudioSource CarAudioSourceForEngine { get; }
    public AudioSource CarAudioSourceForBooster { get; }
    public AudioSource CarAudioSourceForOther { get; }
    public ReactiveProperty<bool> SoundReactiveProperty { get; }
    public event Action OnSoundChange; 
}