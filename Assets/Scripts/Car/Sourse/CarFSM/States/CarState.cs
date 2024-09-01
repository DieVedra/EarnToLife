using UniRx;
using UnityEngine;

public abstract class CarState
{
    protected WheelJoint2D FrontWheelJoint;
    protected WheelJoint2D BackWheelJoint;
    // protected Booster Booster;
    protected PropulsionUnit PropulsionUnit;
    protected bool CarBroken = false;
    protected CarState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit, ReactiveCommand onCarBrokenIntoTwoParts)
    {
        FrontWheelJoint = frontWheelJoint;
        BackWheelJoint = backWheelJoint;
        // Booster = booster;
        PropulsionUnit = propulsionUnit;
        onCarBrokenIntoTwoParts.Subscribe(_ => { CarBrokenIntoTwoParts();});
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public virtual void Dispose(){}
    public virtual void Exit(){}
    protected abstract void SetMotorSpeed(WheelJoint2D wheelJoint);
    private void CarBrokenIntoTwoParts()
    {
        CarBroken = true;
    }
}