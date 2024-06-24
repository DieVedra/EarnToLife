using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ControlCar
{
    private readonly InputMethod _currentInputMethod;
    private readonly Gyroscope _gyroscope;
    private readonly IStateSetter _stateSetterPropulsionUnit;
    private readonly PropulsionUnit _propulsionUnit;
    private readonly Booster _booster;
    private readonly Speedometer _speedometer;
    public ReactiveCommand DriveStarted = new ReactiveCommand();
    private int _stayValue = 5;

    protected ControlCar(InputMethod currentInputMethod, Gyroscope gyroscope, IStateSetter stateSetterPropulsionUnit, PropulsionUnit propulsionUnit,
        Booster booster, Speedometer speedometer)
    {
        _gyroscope = gyroscope;
        _stateSetterPropulsionUnit = stateSetterPropulsionUnit;
        _propulsionUnit = propulsionUnit;
        _booster = booster;
        _speedometer = speedometer;
        _currentInputMethod = currentInputMethod;
        ChangeStaySpeedAfterStartGame();
    }
    public virtual void Update()
    {
        if (_currentInputMethod.CheckPressBreak())
        {
            FirstPress();
            Stop();
            return;
        }
        else if (_currentInputMethod.CheckPressBoost() && _booster.FuelAvailability == true)
        {
            FirstPress();
            Boost();
        }
        else if (_currentInputMethod.CheckPressGas() && _propulsionUnit.FuelAvailability == true)
        {
            FirstPress();
            Gas(_currentInputMethod.SmoothnessTimerForGas.Value);
            return;
        }
        else if (_speedometer.CurrentSpeedInt > _stayValue)
        {
            Roll();
        }
        else
        {
            Stop();
        }
        if (_currentInputMethod.CheckPressRotation())
        {
            Rotation(_currentInputMethod.SmoothnessTimerForRotation.Value);
        }
    }

    public virtual void TryTurnOffCheckBooster() { }
    public virtual void TryTurnOffCheckGun() { }
    private void Gas(float value)
    {
        _propulsionUnit.Transmission.Direction = value;
        _stateSetterPropulsionUnit.SetState<GasState>();
    }
    private void Stop()
    {
        _stateSetterPropulsionUnit.SetState<StopState>();
    }
    private void Roll()
    {
        _stateSetterPropulsionUnit.SetState<RollState>();
    }
    private void Boost()
    {
        _stateSetterPropulsionUnit.SetState<FlyState>();
    }
    private void Rotation(float value)
    {
        _gyroscope.Rotation(value);
    }
    private async void ChangeStaySpeedAfterStartGame()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        _stayValue = 0;
    }

    private void FirstPress()
    {
        if (DriveStarted != null)
        {
            DriveStarted.Execute();
            // DriveStarted.Dispose();
            // DriveStarted = null;
        }
    }
}