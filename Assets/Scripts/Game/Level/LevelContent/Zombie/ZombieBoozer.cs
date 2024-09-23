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
    
    private new void Awake()
    {
        _mainModule = _fartEffect.main;
        _mainModule.duration = _duration;
        _mainModule.loop = true;
        _zombieBoozerAudioHandler = new ZombieBoozerAudioHandler(GetComponent<AudioSource>(),
            GlobalAudio.SoundReactiveProperty, GlobalAudio.AudioPauseReactiveProperty, AudioClipProvider.LevelAudioClipProvider);
        base.Awake();
    }
    
    private void StopEffectImmediately()
    {
        _fartEffect.Stop();
        _zombieBoozerAudioHandler.StopCyclePlaySound();
    }

    private void StopEffect()
    {
        _zombieBoozerAudioHandler.StopCyclePlaySound();
    }
    private void PlayEffect()
    {
        _fartEffect.Play();
        _zombieBoozerAudioHandler.PlayFart();
    }
    
    private new void OnEnable()
    {
        _zombieBoozerAudioHandler.StartCyclePlaySound(PlayEffect, _duration);
        OnBroken += StopEffect;
        GameOverSignal.OnGameOver += StopEffectImmediately;
        base.OnEnable();
    }
    
    private new void OnDisable()
    {
        if (_gameOverSignal != null)
        {
            GameOverSignal.OnGameOver -= StopEffectImmediately;
        }
        OnBroken -= StopEffect;
        StopEffect();
        base.OnDisable();
    }
}