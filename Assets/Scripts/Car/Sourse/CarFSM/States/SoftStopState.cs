using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class SoftStopState : CarState
{
    private const bool USEMOTOR = true;
    private readonly float _duration = 1.5f;
    private readonly float _targetValue = 0f;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private float _currentSpeedValue;
    private float _startDecreaseSpeedValue;
    public SoftStopState(WheelJoint2D frontWheelJoint, WheelJoint2D backWheelJoint, PropulsionUnit propulsionUnit,
        Booster booster, ReactiveCommand onCarBrokenIntoTwoParts)
        : base(frontWheelJoint, backWheelJoint, propulsionUnit, booster, onCarBrokenIntoTwoParts) { }

    public override void Enter()
    {
        DecreaseSpeed(FrontWheelJoint);
        if (CarBroken == false)
        {
            DecreaseSpeed(BackWheelJoint);
        }
        Booster?.TryStopBooster();
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        SetMotorSpeed(FrontWheelJoint);
        SetMotorSpeed(BackWheelJoint);
    }

    public override void Exit()
    {
        _cancellationTokenSource.Cancel();
    }

    protected override void SetMotorSpeed(WheelJoint2D wheelJoint)
    {
        JointMotor2D motor;
        motor = wheelJoint.motor;
        motor.motorSpeed = _currentSpeedValue;
        wheelJoint.motor = motor;
        wheelJoint.useMotor = USEMOTOR;
    }

    private void DecreaseSpeed(WheelJoint2D wheelJoint)
    {
        JointMotor2D motor;
        motor = wheelJoint.motor;
        _startDecreaseSpeedValue = motor.motorSpeed;
        DOTween.To(() => _startDecreaseSpeedValue, x => _currentSpeedValue = x, _targetValue, _duration)
            .SetUpdate(true)
            .SetEase(Ease.InCubic).WithCancellation(_cancellationTokenSource.Token);
    }
}