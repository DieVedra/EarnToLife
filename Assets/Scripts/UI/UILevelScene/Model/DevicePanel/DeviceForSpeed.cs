using UniRx;
using UnityEngine;

public class DeviceForSpeed : Device
{
    private Speedometer _speedometer;

    public DeviceForSpeed(RectTransform transformArrow, Speedometer speedometer, CompositeDisposable compositeDisposable, float angleMin, float angleMax, float maxSpeed)
        : base(transformArrow, compositeDisposable, angleMin, angleMax, maxSpeed)
    {
        _speedometer = speedometer;
        SubscribeUpdate();
    }
    private void SpeedArrowChanged(float speed)
    {
        TransformArrow.localEulerAngles = new Vector3(0f, 0f,
            CalculateAngle(AngleMin, AngleMax, speed / MaxValue));
    }
    private void SubscribeUpdate()
    {
        Observable.EveryUpdate().Subscribe(_ =>
            {
                SpeedArrowChanged(_speedometer.CurrentSpeedFloat);
            }).AddTo(CompositeDisposable);
    }
}