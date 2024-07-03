
using UnityEngine;

public class TimeScalerValues
{
    public readonly Vector2 DownDurationRange;
    public readonly Vector2 UpDurationRange;
    public readonly Vector2 DownTargetTimeRange;
    public const float MIN_VALUE_TIME = 0f;
    public const float NORMAL_VALUE_TIME = 1f;

    public TimeScalerValues(Vector2 downDurationRange, Vector2 upDurationRange, Vector2 downTargetTimeRange)
    {
        DownDurationRange = downDurationRange;
        UpDurationRange = upDurationRange;
        DownTargetTimeRange = downTargetTimeRange;
    }
}