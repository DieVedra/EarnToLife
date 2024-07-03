
public abstract class TimeScalerState
{
    protected readonly TimeScalerValues TimeScalerValues;
    protected readonly TimeScaleSignal TimeScaleSignal;
    protected TimeScalerState(TimeScalerValues timeScalerValues, TimeScaleSignal timeScaleSignal)
    {
        TimeScalerValues = timeScalerValues;
        TimeScaleSignal = timeScaleSignal;
    }

    public abstract void Enter();
    public abstract void Exit();
}