using UniRx;
using UnityEngine;

public class FlyState : CarState
{
    private readonly bool _useMotor = false;
    private readonly Booster _booster;
    public FlyState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit,
        Booster booster, ReactiveCommand onCarBrokenIntoTwoParts)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, onCarBrokenIntoTwoParts)
    {
        _booster = booster;
    }
    public override void Enter()
    {
        SetMotorSpeed(FrontWheelJoint);
        if (CarBroken == false)
        {
            SetMotorSpeed(BackWheelJoint);
        }
        _booster.RunBooster();
    }

    public override void Update()
    {
        BreakingEngine();
    }

    public override void FixedUpdate()
    {
        _booster.Update();
        _booster.BoosterFuelTank.BurnBoosterFuelOnFly();
    }

    public override void Exit()
    {
        _booster?.TryStopBooster();
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