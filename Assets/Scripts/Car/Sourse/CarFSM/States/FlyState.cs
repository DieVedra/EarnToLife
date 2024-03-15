using UniRx;
using UnityEngine;

public class FlyState : CarState
{
    private readonly float _force;
    private readonly bool _useMotor = false;
    private readonly Rigidbody2D _rigidbody;
    private readonly Transform _transform;
    public FlyState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit,
        Booster booster, Rigidbody2D rigidbody, ReactiveCommand onCarBrokenIntoTwoParts, float force)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster, onCarBrokenIntoTwoParts)
    {
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
        Booster.RunBooster();
    }

    public override void Update()
    {
        BreakingEngine();
        Booster.BoosterFuelTank.BurnBoosterFuelOnFly();
        MoveFly();
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