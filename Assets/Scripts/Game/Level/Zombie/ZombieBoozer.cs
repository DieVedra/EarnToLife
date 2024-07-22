using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ZombieBoozer : Zombie
{
    [SerializeField] private ParticleSystem _fartEffect;
    [SerializeField] private float _duration = 5f;
    private ParticleSystem.MainModule _mainModule;
    private ZombieBoozerAudioHandler _zombieBoozerAudioHandler;
    private GameOverSignal _gameOverSignal;
    [Inject]
    private void Construct(AudioClipProvider audioClipProvider, GameOverSignal gameOverSignal, IGlobalAudio globalAudio)
    {
        _zombieBoozerAudioHandler = new ZombieBoozerAudioHandler(GetComponent<AudioSource>(),
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty, audioClipProvider.LevelAudioClipProvider);
        _gameOverSignal = gameOverSignal;
        _gameOverSignal.OnGameOver += StopEffect;
    }
    private void Awake()
    {
        _mainModule = _fartEffect.main;
        _mainModule.duration = _duration;
        _mainModule.loop = true;
        
    }
    private void StopEffect()
    {
        _fartEffect.Stop();
    }

    private void PlayEffect()
    {
        _fartEffect.Play();
        _zombieBoozerAudioHandler.PlayFart();

    }
    private void OnEnable()
    {
        StartCoroutine(StartCyclePlaySound(PlayEffect, _duration));
        OnBroken += StopEffect;
    }
    private void OnDisable()
    {
        _gameOverSignal.OnGameOver -= StopEffect;
        OnBroken -= StopEffect;
        StopEffect();
    }
}