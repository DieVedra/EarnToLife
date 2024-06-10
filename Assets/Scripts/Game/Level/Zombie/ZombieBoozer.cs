using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ZombieBoozer : Zombie
{
    [SerializeField] private ParticleSystem _fartEffect;
    [SerializeField] private float _duration = 5f;
    private ParticleSystem.MainModule _mainModule;
    private ZombieBoozerAudioHandler _zombieBoozerAudioHandler;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    [Inject]
    private void Construct(AudioClipProvider audioClipProvider, IGlobalAudio globalAudio)
    {
        _zombieBoozerAudioHandler = new ZombieBoozerAudioHandler(GetComponent<AudioSource>(),
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty, audioClipProvider.LevelAudioClipProvider);
    }
    private void Start()
    {
        _mainModule = _fartEffect.main;
        _mainModule.duration = _duration;
        _mainModule.loop = true;
        StartCyclePlaySound(_cancellationTokenSource, PlayEffect,
            _duration, _duration).Forget();
        OnBroken += StopEffect;
    }
    private void StopEffect()
    {
        _fartEffect.Stop();
        _cancellationTokenSource.Cancel();
    }

    private void PlayEffect()
    {
        _fartEffect.Play();
        _zombieBoozerAudioHandler.PlayFart();

    }
    private new void OnDisable()
    {
        OnBroken -= StopEffect;
        StopEffect();
    }
}