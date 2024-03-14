using UnityEngine;

public class StopState : CarState
{
    private const float STOPSPEED = 0f;
    private const bool USEMOTOR = true;
    private Brakes _brakes;
    public StopState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit, Brakes brakes, Booster booster)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster)
    {
        _brakes = brakes;
    }
    public override void Enter()
    {
        SetMotorSpeed(FrontWheelJoint);
        if (BackWheelJoint != null)
        {
            SetMotorSpeed(BackWheelJoint);
        }
        Booster?.TryStopBooster();
    }
    public override void Update()
    {
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
