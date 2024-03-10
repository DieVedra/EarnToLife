using System;
using UnityEngine;

public interface ICarAudio
{
    public CarClips CarClips { get; }
    public AudioSource CarAudioSource1 { get; }
    public AudioSource CarAudioSource2 { get; }
    public bool SoundOn { get; }
    public event Action OnSoundChange; 
}