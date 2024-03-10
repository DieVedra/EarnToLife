using UniRx;
using UnityEngine;

public class Device
{
    protected readonly float AngleMin;
    protected readonly float AngleMax;
    protected readonly float MaxValue;
    protected RectTransform TransformArrow;
    protected CompositeDisposable CompositeDisposable;

    protected Device(RectTransform transformArrow, CompositeDisposable compositeDisposable, float angleMin, float angleMax, float maxValue)
    {
        TransformArrow = transformArrow;
        CompositeDisposable = compositeDisposable;
        AngleMin = angleMin;
        AngleMax = angleMax;
        MaxValue = maxValue;
    }
    protected float CalculateAngle(float min, float max, float value)
    {
        return Mathf.Lerp(min, max, value);
    }
}