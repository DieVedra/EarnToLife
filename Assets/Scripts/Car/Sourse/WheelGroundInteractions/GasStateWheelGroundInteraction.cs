using UniRx;
using UnityEngine;

public class GasStateWheelGroundInteraction : WheelGroundInteraction
{
    private readonly Vector3 _forwardRotation = new Vector3(0f, 0f,0f);
    private readonly Vector3 _backwardRotation = new Vector3(0f, 180f,0f);
    private readonly Transmission _transmission;
    private bool IsMovementForward => _transmission.IsMovementForward;
    public GasStateWheelGroundInteraction(GroundAnalyzer groundAnalyzer, Speedometer speedometer, Transmission transmission,
        CarWheel frontCarWheel, CarWheel backCarWheel, AnimationCurve particlesSpeedCurve) 
        : base(groundAnalyzer, speedometer, frontCarWheel, backCarWheel, particlesSpeedCurve)
    {
        _transmission = transmission;
    }
    public override void Init()
    {
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnGroundReactiveProperty, SetRotation);
        SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnGroundReactiveProperty, SetRotation);
        base.Init();
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