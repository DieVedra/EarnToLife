using UnityEngine;

public class StopStateWheelGroundInteraction : WheelGroundInteraction
{
    private readonly Vector3 _stopRotation = new Vector3(0f, 90f,0f);
    public StopStateWheelGroundInteraction(GroundAnalyzer groundAnalyzer, Speedometer speedometer, 
        CarWheel frontCarWheel, CarWheel backCarWheel, AnimationCurve particlesSpeedCurve)
    :base(groundAnalyzer, speedometer, frontCarWheel, backCarWheel, particlesSpeedCurve)
    {
        
    }
    public override void Init()
    {
        SubscribeReactiveProperty(GroundAnalyzer.FrontWheelOnGroundReactiveProperty, SetRotation);
        SubscribeReactiveProperty(GroundAnalyzer.BackWheelOnGroundReactiveProperty, SetRotation);
        base.Init();
    }
    
    protected override void SetRotation()
    {
        FrontWheelDirtEffectTransform.rotation = Quaternion.Euler(_stopRotation);
        BackWheelDirtEffectTransform.rotation = Quaternion.Euler(_stopRotation);
    }
}