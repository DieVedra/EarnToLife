using UniRx;
using UnityEngine;

public class FlyState : CarState
{
    private readonly float _force;
    private readonly bool _useMotor = false;
    private readonly Booster _booster;
    private readonly Rigidbody2D _rigidbody;
    private readonly Transform _transform;
    public FlyState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit,
        Booster booster, Rigidbody2D rigidbody, ReactiveCommand onCarBrokenIntoTwoParts, float force)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, onCarBrokenIntoTwoParts)
    {
        _booster = booster;
        _rigidbody = rigidbody;
        _transform = rigidbody.transform;
        _force = force;
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
        MoveFly();
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
    private void MoveFly()
    {
        _rigidbody.AddForce(_transform.right * _force);
    }
}