using System;

public class TimeScaleSignal
{
    public Action<float> OnTimeScaleChange;
    public Action OnTimeWarpedOff;
    public Action OnTimeWarpedOn;
}