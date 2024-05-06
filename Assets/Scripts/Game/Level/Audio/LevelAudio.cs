using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LevelAudio : MonoBehaviour
{
    private AudioSource _levelAudioSource;
    private LevelAudioClipProvider _levelAudioClipProvider;
    private LevelAudioHandler _levelAudioHandler;

    public WoodDestructibleAudioHandler WoodDestructibleAudioHandler => _levelAudioHandler.WoodDestructibleAudioHandler;
    public BarrelAudioHandler BarrelAudioHandler => _levelAudioHandler.BarrelAudioHandler;
    public void Init(AudioClipProvider audioClipProvider, IGlobalAudio globalAudio)
    {
        _levelAudioSource = GetComponent<AudioSource>();
        _levelAudioHandler = new LevelAudioHandler(_levelAudioSource, globalAudio, audioClipProvider.LevelAudioClipProvider);
    }
}