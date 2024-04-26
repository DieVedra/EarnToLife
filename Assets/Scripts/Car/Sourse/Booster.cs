using System;
using UniRx;
using UnityEngine;

public class Booster
{
    private readonly BoosterScrew _boosterScrew;
    private readonly BoosterAudioHandler _boosterAudioHandler;
    private readonly ParticleSystem _particleSystemBooster;
    private bool _isRun = false;
    public BoosterFuelTank BoosterFuelTank { get; private set; }
    private CompositeDisposable _compositeDisposable => _boosterScrew.CompositeDisposable;
    public bool FuelAvailability => BoosterFuelTank.CheckFuel();
    public event Action OnBoosterDisable;
    public Booster(BoosterAudioHandler boosterAudioHandler, BoosterFuelTank boosterFuelTank,
        BoosterScrew boosterScrew, ParticleSystem particleSystemBooster)
    {
        _boosterAudioHandler = boosterAudioHandler;
        BoosterFuelTank = boosterFuelTank;
        _boosterScrew = boosterScrew;
        _particleSystemBooster = particleSystemBooster;
    }
    public void TryStopBooster()
    {
        if (_isRun == true)
        {
            StopBoosterDecrease();
        }
    }
    public void StopBoosterOnOutFuel()
    {
        if (_isRun == true)
        {
            StopBooster();
            _boosterAudioHandler.PlayBoosterEndFuel();
        }
    }
    public void RunBooster()
    {
        _isRun = true;
        _boosterAudioHandler.PlayRunBooster();
        _compositeDisposable.Clear();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _boosterScrew.RotateScrew();
        }).AddTo(_compositeDisposable);
        _particleSystemBooster.Play();
    }
    public void BoosterDisable()
    {
        if (_isRun == true)
        {
            _particleSystemBooster.Stop();
            _boosterAudioHandler.StopPlayRunBoosterImmediately();
        }
        _isRun = false;
        _compositeDisposable.Clear();
        OnBoosterDisable?.Invoke();
    }

    private void StopBoosterDecrease()
    {
        StopBooster();
        _boosterAudioHandler.StopPlayRunBoosterDecrease();
    }
    private void StopBooster()
    {
        _isRun = false;
        _compositeDisposable.Clear();
        _boosterScrew.SetDefaultRotationSpeed();
        _particleSystemBooster.Stop();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _boosterScrew.SmoothStopScrew();
        }).AddTo(_compositeDisposable);
        
    }
}