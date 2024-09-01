using UniRx;
using UnityEngine;

public class GasStateWheelGroundInteraction : WheelGroundInteraction
{
    private readonly Vector3 _forwardRotation = new Vector3(0f, 0f,0f);
    private readonly Vector3 _backwardRotation = new Vector3(0f, 180f,0f);
    private readonly Transmission _transmission;
    private bool IsMovementForward => _transmission.IsMovementForward;
    public GasStateWheelGroundInteraction(GroundAnalyzer groundAnalyzer, Speedometer speedometer, Transmission transmission,
        CarWheel frontCarWheel, CarWheel backCarWheel, AnimationCurve particlesSpeedCurve, ReactiveCommand onCarBrokenIntoTwoParts) 
        : base(groundAnalyzer, speedometer, frontCarWheel, backCarWheel, particlesSpeedCurve, onCarBrokenIntoTwoParts)
    {
        _transmission = transmission;
    }
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
    protected override void SetRotation()
    {
        if (IsMovementForward == true)
        {
            FrontWheelDirtEffectTransform.rotation  = Quaternion.Euler(_forwardRotation);
            BackWheelDirtEffectTransform.rotation  = Quaternion.Euler(_forwardRotation);
        }
        else
        {
            FrontWheelDirtEffectTransform.rotation = Quaternion.Euler(_backwardRotation);
            BackWheelDirtEffectTransform.rotation = Quaternion.Euler(_backwardRotation);
        }
    }
}