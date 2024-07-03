
using System;
using UniRx;
using UnityEngine;

public class TimeScalePitchHandler
{
    private readonly TimeScaleSignal _timeScaleSignal;
    private float _maxPitch = 1f;
    public bool IsTimeWarped => IsTimeWarpedRP.Value;
    public ReactiveProperty<bool> IsTimeWarpedRP = new ReactiveProperty<bool>();

    public event Action<float> OnPitchTimeWarped; 
    public TimeScalePitchHandler(TimeScaleSignal timeScaleSignal)
    {
        _timeScaleSignal = timeScaleSignal;
        _timeScaleSignal.OnTimeScaleChange += PitchChangeFromTimeScale;

        _timeScaleSignal.OnTimeWarpedOff += TimeWarpedOff;
        _timeScaleSignal.OnTimeWarpedOn += TimeWarpedOn;
    }
    public void Dispose()
    {
        _timeScaleSignal.OnTimeScaleChange -= PitchChangeFromTimeScale;
        _timeScaleSignal.OnTimeWarpedOff -= TimeWarpedOff;
        _timeScaleSignal.OnTimeWarpedOn -= TimeWarpedOn;
        IsTimeWarpedRP.Dispose();
    }

    public void SetPitchValueNormalTimeScale(float normalPitch)
    {
        _maxPitch = normalPitch;
    }
    private void TimeWarpedOff()
    {
        IsTimeWarpedRP.Value = false;
    }
    private void TimeWarpedOn()
    {
        IsTimeWarpedRP.Value = true;
    }
    private void PitchChangeFromTimeScale(float timeValue)
    {
        OnPitchTimeWarped?.Invoke(Mathf.LerpUnclamped(0f, _maxPitch, timeValue));
    }
}