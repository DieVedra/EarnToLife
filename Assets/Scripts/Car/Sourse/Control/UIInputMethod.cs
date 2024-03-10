using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class UIInputMethod : InputMethod
{
    private bool _isGas;
    private bool _isBoost;
    private bool _isStop;
    private bool _isRotate;
    public UIInputMethod() 
    {
        SmoothnessTimerForGas = new SmoothnessTimer();
        SmoothnessTimerForRotation = new SmoothnessTimer();
    }
    public void TurnOnGas()
    {
        _isGas = true;
        SmoothnessTimerForGas.StartTimer(DIRECTION_FORWARD);
    }
    public void TurnOffGas()
    {
        _isGas = false;
        SmoothnessTimerForGas.StopTimer();
    }
    public void TurnOnBooster()
    {
        _isBoost = true;
    }
    public void TurnOffBooster()
    {
        _isBoost = false;
    }
    public void TurnOnStop()
    {
        _isStop = true;
        TurnOffGas();
    }
    public void TurnOffStop()
    {
        _isStop = false;
    }
    public void TurnOnRotateClockwise()
    {
        _isRotate = true;
        SmoothnessTimerForRotation.StartTimer(ROTATION_CLOCKWISE);
    }
    public void TurnOnRotateAntiClockwise()
    {
        _isRotate = true;
        SmoothnessTimerForRotation.StartTimer(ROTATION_COUNTER_CLOCKWISE);
    }
    public void TurnOffRotation()
    {
        _isRotate = false;
        SmoothnessTimerForRotation.StopTimer();
    }
    public override bool CheckPressGas()
    {
        return _isGas;
    }
    public override bool CheckPressBreak()
    {
        return _isStop;
    }
    public override bool CheckPressRotation()
    {
        return _isRotate;
    }
    public override bool CheckPressBoost()
    {
        return _isBoost;
    }
}