using System;
using UniRx;
using UnityEngine;

public class Booster
{
    private readonly BoosterScrew _boosterScrew;
    private readonly BoosterAudioHandler _boosterAudioHandler;
    private readonly ParticleSystem _particleSystemBooster;
    private bool _isRun = false;
    private bool _isBroken = false;
    public BoosterFuelTank BoosterFuelTank { get; private set; }
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
            StopBooster();
            _boosterAudioHandler.SetDecreaseBooster();
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

    public void Update()
    {
        if (_isRun == true)
        {
            _boosterScrew.RotateScrew();
        }

        if (_boosterScrew.IsSmoothStop == true)
        {
            _boosterScrew.SmoothStopScrew();
        }

        if (_boosterAudioHandler.VolumeIncreaseValue == true)
        {
            _boosterAudioHandler.VolumeIncrease();
        }

        if (_boosterAudioHandler.VolumeDecreaseValue == true)
        {
            _boosterAudioHandler.VolumeDecrease();
        }
    }
    public void RunBooster()
    {
        if (_isBroken == false)
        {
            _isRun = true;
            _boosterAudioHandler.SetAudioIncreaseBooster();
            _particleSystemBooster.Play();
        }
    }
    public void BoosterDisable()
    {
        _particleSystemBooster.Stop();
        _boosterAudioHandler.StopPlayRunBoosterImmediately();
        _isBroken = true;
        _isRun = false;
        OnBoosterDisable?.Invoke();
    }
    private void StopBooster()
    {
        _isRun = false;
        _boosterScrew.IsSmoothStop = true;
        _boosterScrew.SetDefaultRotationSpeed();
        _particleSystemBooster.Stop();
    }
}