using UnityEngine;

public class RollState : CarState
{
    private readonly bool _useMotor = false;
    public RollState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit, Booster booster)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster) { }
    public override void Enter()
    {
        SetMotorSpeed(FrontWheelJoint);
        SetMotorSpeed(BackWheelJoint);
        Booster?.TryStopBooster();
    }
    public override void Update()
    {
        BreakingEngine();
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
