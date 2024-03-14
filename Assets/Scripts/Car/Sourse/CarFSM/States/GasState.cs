using UnityEngine;

public class GasState : CarState
{
    private const bool USEMOTOR = true;
    private float _currentSpeed;
    private float _newSpeed;
    public GasState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit, Booster booster)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster) { }
    public override void Enter()
    {
        Booster?.TryStopBooster();
    }
    public override void Update()
    {
        AccelerationEngine();
        CalculateSpeed();
        SetMotorSpeed(FrontWheelJoint);
        if (BackWheelJoint != null)
        {
            SetMotorSpeed(BackWheelJoint);
        }
        PropulsionUnit.FuelTank.BurnFuelOnMoving();
        SmoothMovement();
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
