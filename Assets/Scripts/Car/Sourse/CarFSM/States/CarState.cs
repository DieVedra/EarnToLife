using UnityEngine;

public abstract class CarState
{
    protected WheelJoint2D FrontWheelJoint;
    protected WheelJoint2D BackWheelJoint;
    protected Booster Booster;
    protected PropulsionUnit PropulsionUnit;
    protected CarState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit, Booster booster)
    {
        FrontWheelJoint = frontWheelJoint;
        BackWheelJoint = backWheelJoint;
        Booster = booster;
        PropulsionUnit = propulsionUnit;
    }
    public abstract void Enter();
    public abstract void Update();
    protected abstract void SetMotorSpeed(WheelJoint2D wheelJoint);
    public virtual void Exit(){}
}