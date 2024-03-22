using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brakes
{
    private const int MINSPEED = 15;
    private readonly GroundAnalyzer _groundAnalyzer;
    private readonly Speedometer _speedometer;
    private readonly CarAudioHandler _carAudioHandler;
    private readonly AnimationCurve _brakeVolumeCurve;
    private bool _isBrake = false;
    private bool GroundContact => _groundAnalyzer.FrontWheelContact || _groundAnalyzer.BackWheelContact;
    public Brakes(CarAudioHandler carAudioHandler, Speedometer speedometer, GroundAnalyzer groundAnalyzer, AnimationCurve brakeVolumeCurve)
    {
        _speedometer = speedometer;
        _groundAnalyzer = groundAnalyzer;
        _carAudioHandler = carAudioHandler;
        _brakeVolumeCurve = brakeVolumeCurve;
    }
    public void BrakeSoundOn()
    {
        if (GroundContact && CheckSpeed())
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
}
