using System;
using UniRx;
using UnityEngine;

public interface ICarAudio
{
    public CarClips CarClips { get; }
    public AudioSource CarAudioSource1 { get; }
    public AudioSource CarAudioSource2 { get; }
    public ReactiveProperty<bool> SoundReactiveProperty { get; }
    public event Action OnSoundChange; 
}