
public class BoosterValues
{
    public readonly float StartIncreaseValue = 0f;
    public readonly float StartDecreaseValue = 1f;
    public float CurrentValue;
    public float CurrentCurveValue;

    public BoosterValues()
    {
        CurrentValue = StartIncreaseValue;
    }
}