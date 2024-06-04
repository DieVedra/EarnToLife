using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ZombieBoozer : Zombie
{
    [SerializeField] private ParticleSystem _fartEffect;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _startDelay = 2f;
    private readonly float _multiplier = 0.5f;
    private ParticleSystem.MainModule _mainModule;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private void Start()
    {
        _mainModule = _fartEffect.main;
        _mainModule.duration = _duration;
        _mainModule.startDelay = _startDelay;
        _mainModule.loop = true;
        _fartEffect.Play();
        StartCyclePlaySound(_cancellationTokenSource,()=>{ZombieAudioHandler.PlayFart();},
            _startDelay + _multiplier, _duration).Forget();
        OnBroken += StopEffect;
    }
    private void StopEffect()
    {
        _fartEffect.Stop();
        _cancellationTokenSource.Cancel();
    }
    private new void OnDisable()
    {
        OnBroken -= StopEffect;
        StopEffect();
    }
}