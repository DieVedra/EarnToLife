using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brakes
{
    private const int MINSPEED = 15;
    private readonly GroundAnalyzer _groundAnalyzer;
    private readonly Speedometer _speedometer;
    private readonly BrakeAudioHandler _brakeAudioHandler;
    private bool GroundContact => _groundAnalyzer.FrontWheelContact || _groundAnalyzer.BackWheelContact;
    public Brakes(BrakeAudioHandler brakeAudioHandler, Speedometer speedometer, GroundAnalyzer groundAnalyzer)
    {
        _speedometer = speedometer;
        _groundAnalyzer = groundAnalyzer;
        _brakeAudioHandler = brakeAudioHandler;
    }
    public void BrakeSoundOn()
    {
        _brakeAudioHandler.SetMuteVolumeBrake();
        TrySetClip();
        if (CheckSpeedAndGroundContact())
        {
            _brakeAudioHandler.PlayBrake();
        }
    }

    public void Update()
    {
        if (CheckSpeedAndGroundContact())
        {
            _brakeAudioHandler.SetVolumeBrake(_speedometer.CurrentSpeedFloat);
            TrySetClip();
        }
        else
        {
            _brakeAudioHandler.SetMuteVolumeBrake();
        }
    }
    public void BrakeSoundOff()
    {
        _brakeAudioHandler.StopPlayBrake();
    }
    private bool CheckSpeedAndGroundContact()
    {
        if (GroundContact == true && _speedometer.CurrentSpeedInt > MINSPEED)
        {
            return true;
        }
        else return false;
    }

    private void TrySetClip()
    {
        if(_groundAnalyzer.FrontWheelOnGroundReactiveProperty.Value == true )
        {
            _brakeAudioHandler.TrySetGroundClip();
        }
        else
        {
            _brakeAudioHandler.TrySetAsphaltClip();
        }
    }
}
