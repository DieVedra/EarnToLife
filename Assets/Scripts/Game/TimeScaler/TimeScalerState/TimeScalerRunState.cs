
using System;
using UnityEngine;

public class TimeScalerRunState : TimeScalerState
{
    public TimeScalerRunState(TimeScalerValues timeScalerValues, TimeScaleSignal timeScaleSignal)
        : base(timeScalerValues, timeScaleSignal) { }

    public override void Enter()
    {
        // Debug.Log($"                            Run");
        Time.timeScale = TimeScalerValues.NORMAL_VALUE_TIME;
        TimeScaleSignal.OnTimeScaleChange?.Invoke(TimeScalerValues.NORMAL_VALUE_TIME);
    }

    public override void Exit() { }
}