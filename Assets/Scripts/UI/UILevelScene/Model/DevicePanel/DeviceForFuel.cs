using UniRx;
using UnityEngine;

public class DeviceForFuel : Device
{
    private Tank _tank;
    private BlinkIndicator _blinkIndicator;
    public DeviceForFuel(RectTransform transformArrow, Tank tank, BlinkIndicator blinkIndicator, CompositeDisposable compositeDisposable,
        float angleMin, float angleMax, float maxFuel)
        : base(transformArrow, compositeDisposable, angleMin, angleMax, maxFuel)
    {
        _tank = tank;
        _blinkIndicator = blinkIndicator;
        SubscribeUpdate();
        
    }
    private void ArrowChange(float currentFuelQuantity, bool tankFullnessIndicator)
    {
        float value = currentFuelQuantity / MaxValue;
        TransformArrow.localEulerAngles 
            = new Vector3(0f, 0f, 
                CalculateAngle(AngleMin, AngleMax, value));
        _blinkIndicator.TryBlinking(value, tankFullnessIndicator);
    }
    private void SubscribeUpdate()
    {
        // Debug.Log("                                       SubscrFl");
        Observable.EveryUpdate().Subscribe(_ =>
        {
            // Debug.Log("ArrowChange");
            ArrowChange(_tank.FuelQuantity, _tank.IsEmpty);
        }).AddTo(CompositeDisposable);
    }
}