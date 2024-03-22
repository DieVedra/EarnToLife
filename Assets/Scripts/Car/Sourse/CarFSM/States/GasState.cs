using UniRx;
using UnityEngine;

public class GasState : CarState
{
    private const bool USEMOTOR = true;
    private readonly GasStateWheelGroundInteraction _gasStateWheelGroundInteraction;
    private float _currentSpeed;
    private float _newSpeed;

    public GasState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit,
        Booster booster, GasStateWheelGroundInteraction gasStateWheelGroundInteraction,
        ReactiveCommand onCarBrokenIntoTwoParts)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster, onCarBrokenIntoTwoParts)
    {
        _gasStateWheelGroundInteraction = gasStateWheelGroundInteraction;
    }
    public override void Enter()
    {
        Booster?.TryStopBooster();
        _gasStateWheelGroundInteraction.Init();
    }
    public override void Update()
    {
        AccelerationEngine();
        CalculateSpeed();
        _gasStateWheelGroundInteraction.Update();
        SetMotorSpeed(FrontWheelJoint);
        if (CarBroken == false)
        {
            SetMotorSpeed(BackWheelJoint);
        }
        PropulsionUnit.FuelTank.BurnFuelOnMoving();
        SmoothMovement();
    }

    public override void Exit()
    {
        _gasStateWheelGroundInteraction.Dispose();
    }

    protected override void SetMotorSpeed(WheelJoint2D wheelJoint)
    {
        JointMotor2D motor;
        motor = wheelJoint.motor;
        motor.motorSpeed = _currentSpeed;
        wheelJoint.motor = motor;
        wheelJoint.useMotor = USEMOTOR;
    }
    private void CalculateSpeed()
    {
        _newSpeed = PropulsionUnit.GetCarSpeed();
    }
    private void AccelerationEngine()
    {
        PropulsionUnit.Engine.AccelerationEngine();
    }
    private void SmoothMovement()
    {
        if (PropulsionUnit.Transmission.IsMovementForward == true)
        {
            if (_newSpeed < _currentSpeed)
            {
                _currentSpeed = _newSpeed;
            }
        }
        else
        {
            if (_newSpeed > _currentSpeed)
            {
                _currentSpeed = _newSpeed;
            }
        }
    }
}
