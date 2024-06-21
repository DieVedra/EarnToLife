using UniRx;
using UnityEngine;

public class RollState : CarState
{
    private readonly bool _useMotor = false;
    public RollState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit,
        Booster booster, ReactiveCommand onCarBrokenIntoTwoParts)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster, onCarBrokenIntoTwoParts) { }
    public override void Enter()
    {
        SetMotorSpeed(FrontWheelJoint);
        if (CarBroken == false)
        {
            SetMotorSpeed(BackWheelJoint);
        }
        Booster?.TryStopBooster();
    }
    public override void Update()
    {
        BreakingEngine();
    }

    public override void FixedUpdate()
    {
        PropulsionUnit.FuelTank.BurnFuelOnIdling();
    }

    private void BreakingEngine()
    {
        PropulsionUnit.Engine.BreakingEngine();
    }
    protected override void SetMotorSpeed(WheelJoint2D wheelJoint)
    {
        wheelJoint.useMotor = _useMotor;
    }
}
