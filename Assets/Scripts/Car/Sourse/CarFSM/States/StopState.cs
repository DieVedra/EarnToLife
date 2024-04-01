using UniRx;
using UnityEngine;

public class StopState : CarState
{
    private const float STOPSPEED = 0f;
    private const bool USEMOTOR = true;
    private readonly StopStateWheelGroundInteraction _stateWheelGroundInteraction;
    private readonly Brakes _brakes;
    public StopState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit,
        Brakes brakes, StopStateWheelGroundInteraction stateWheelGroundInteraction, Booster booster, ReactiveCommand onCarBrokenIntoTwoParts)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster, onCarBrokenIntoTwoParts)
    {
        _brakes = brakes;
        _stateWheelGroundInteraction = stateWheelGroundInteraction;
    }
    public override void Enter()
    {
        SetMotorSpeed(FrontWheelJoint);
        if (CarBroken == false)
        {
            SetMotorSpeed(BackWheelJoint);
        }
        _stateWheelGroundInteraction.Init(CarBroken);
        Booster?.TryStopBooster();
        _brakes.BrakeSoundOn();

    }
    public override void Update()
    {
        _stateWheelGroundInteraction.Update();
        BreakingEngine();
        _brakes.Update();
        PropulsionUnit.FuelTank.BurnFuelOnIdling();
    }

    public override void Exit()
    {
        _brakes.BrakeSoundOff();
        _stateWheelGroundInteraction.Dispose();
    }
    protected override void SetMotorSpeed(WheelJoint2D wheelJoint)
    {
        JointMotor2D motor;
        motor = wheelJoint.motor;
        motor.motorSpeed = STOPSPEED;
        wheelJoint.motor = motor;
        wheelJoint.useMotor = USEMOTOR;
    }

    private void BreakingEngine()
    {
        PropulsionUnit.Engine.BreakingEngine();
    }
}
