using UnityEngine;

public class ExhaustHandler
{
    private readonly Transform _point1;
    private readonly Transform _point2;
    private readonly Exhaust _exhaust;
    public ExhaustHandler(Exhaust exhaust, Transform point1, Transform point2)
    {
        _point1 = point1;
        _point2 = point2;
        _exhaust = exhaust;
    }

    public void SetPoint1()
    {
        _exhaust.EffectTransform.transform.position = _point1.transform.position;
    }
    public void SetPoint2()
    {
        _exhaust.EffectTransform.transform.position = _point2.transform.position;
    }
}