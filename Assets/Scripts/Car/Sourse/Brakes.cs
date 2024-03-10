using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brakes
{
    private const int MINSPEED = 15;
    private CarWheel _frontWheel;
    private CarWheel _backWheel;
    private Speedometer _speedometer;
    private CarAudioHandler _carAudioHandler;
    private LayerMask _ground;
    private AnimationCurve _brakeVolumeCurve;
    private bool _isBrake = false;
    public Brakes(CarAudioHandler carAudioHandler, Speedometer speedometer, CarWheel frontWheel, CarWheel backWheel, LayerMask ground, AnimationCurve brakeVolumeCurve)
    {
        _speedometer = speedometer;
        _carAudioHandler = carAudioHandler;
        _frontWheel = frontWheel;
        _backWheel = backWheel;
        _ground = ground;
        _brakeVolumeCurve = brakeVolumeCurve;
    }
    public void BrakeSoundOn()
    {
        if (CheckGround() && CheckSpeed())
        {
            if (_isBrake == false)
            {
                _isBrake = true;
                _carAudioHandler.PlayBrake();
            }
            _carAudioHandler.SetVolumeBrake(_brakeVolumeCurve.Evaluate(_speedometer.CurrentSpeedFloat));
        }
        else
        {
            BrakeSoundOff();
        }
    }
    public void BrakeSoundOff()
    {
        _isBrake = false;
        _carAudioHandler.StopPlayBrake();
    }
    private bool CheckSpeed()
    {
        if (_speedometer.CurrentSpeedInt > MINSPEED)
        {
            return true;
        }
        else return false;
    }
    private bool CheckGround()
    {
        if (CheckCircle(_backWheel))
        {
            return true;
        }
        else if (CheckCircle(_frontWheel))
        {
            return true;
        }
        else return false;
    }

    private bool CheckCircle(CarWheel wheel)
    {
        if (Physics2D.OverlapCircle(wheel.Position, wheel.Radius, _ground.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
