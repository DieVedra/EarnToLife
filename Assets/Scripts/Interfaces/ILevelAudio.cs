using System;
using UniRx;
using UnityEngine;

public interface ILevelAudio
{
    public LevelAudioClipProvider LevelAudioClipProvider { get; }
    public AudioSource LevelAudioSource { get; }
    public ReactiveProperty<bool> SoundReactiveProperty { get; }
    public event Action OnSoundChange; 
}