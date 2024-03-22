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
        Booster?.TryStopBooster();
    }
    public override void Update()
    {
        _stateWheelGroundInteraction.Update();
        BreakingEngine();
        SoundBrake();
        PropulsionUnit.FuelTank.BurnFuelOnIdling();
    }

    public override void Exit()
    {
        _brakes.BrakeSoundOff();
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

    private void SoundBrake()
    {
        _brakes.BrakeSoundOn();
    }
}
