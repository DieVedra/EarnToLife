
using UnityEngine;

public class TimeScalerStopState : TimeScalerState
{
    public TimeScalerStopState(TimeScalerValues timeScalerValues, TimeScaleSignal timeScaleSignal)
        : base(timeScalerValues, timeScaleSignal) { }

    public override void Enter()
    {
        Debug.Log($"                            Stop");
        Time.timeScale = TimeScalerValues.MIN_VALUE_TIME;
        TimeScaleSignal.OnTimeScaleChange?.Invoke(TimeScalerValues.MIN_VALUE_TIME);
    }

    public override void Exit() { }
}