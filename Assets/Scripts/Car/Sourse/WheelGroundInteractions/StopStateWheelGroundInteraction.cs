using UniRx;
using UnityEngine;

public class StopStateWheelGroundInteraction : WheelGroundInteraction
{
    private readonly Vector3 _stopRotation = new Vector3(0f, 90f,0f);
    private readonly float _minSpeedValue = 5f;
    public StopStateWheelGroundInteraction(GroundAnalyzer groundAnalyzer, Speedometer speedometer, 
        CarWheel frontCarWheel, CarWheel backCarWheel, AnimationCurve particlesSpeedCurve, ReactiveCommand onCarBrokenIntoTwoParts)
    :base(groundAnalyzer, speedometer, frontCarWheel, backCarWheel, particlesSpeedCurve, onCarBrokenIntoTwoParts) { }
    public override void Enter(bool carBroken)
    {
        CompositeDisposableFrontWheel = new CompositeDisposable();
        CompositeDisposableBackWheel = new CompositeDisposable();
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnGroundReactiveProperty, SetRotation, CompositeDisposableFrontWheel);
        if (carBroken == false)
        {
            SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnGroundReactiveProperty, SetRotation, CompositeDisposableBackWheel);
        }
        SubscribeReactiveProperty(Speedometer.CurrentSpeedReactiveProperty, ChangesParticlesSpeeds, CompositeDisposableFrontWheel);
        base.Enter(carBroken);
    }

    protected override void ChangesParticlesSpeeds()
    {
        if (Speedometer.CurrentSpeedFloat > _minSpeedValue)
        {
            base.ChangesParticlesSpeeds();
        }
        else
        {
            StopEffects();
        }
    }

    protected override void SetRotation()
    {
        FrontWheelDirtEffectTransform.rotation = Quaternion.Euler(_stopRotation);
        BackWheelDirtEffectTransform.rotation = Quaternion.Euler(_stopRotation);
    }
}