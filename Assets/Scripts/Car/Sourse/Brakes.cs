using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brakes
{
    private const int MINSPEED = 15;
    private readonly GroundAnalyzer _groundAnalyzer;
    private readonly Speedometer _speedometer;
    private readonly BrakeAudioHandler _brakeAudioHandler;
    private readonly AnimationCurve _brakeVolumeCurve;
    private bool _isBrake = false;
    private bool GroundContact => _groundAnalyzer.FrontWheelContact || _groundAnalyzer.BackWheelContact;
    public Brakes(BrakeAudioHandler brakeAudioHandler, Speedometer speedometer, GroundAnalyzer groundAnalyzer, AnimationCurve brakeVolumeCurve)
    {
        _speedometer = speedometer;
        _groundAnalyzer = groundAnalyzer;
        _brakeAudioHandler = brakeAudioHandler;
        _brakeVolumeCurve = brakeVolumeCurve;
    }
    public void BrakeSoundOn()
    {
        if (GroundContact && CheckSpeed())
        {
            if (_isBrake == false)
            {
                _isBrake = true;
                _brakeAudioHandler.PlayBrake();
            }
            _brakeAudioHandler.SetVolumeBrake(_brakeVolumeCurve.Evaluate(_speedometer.CurrentSpeedFloat));
        }
        else
        {
            BrakeSoundOff();
        }
    }
    public void BrakeSoundOff()
    {
        _isBrake = false;
        _brakeAudioHandler.StopPlayBrake();
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
