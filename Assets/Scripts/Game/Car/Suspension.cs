using System;
using UnityEngine;

public class Suspension
{
    private readonly float _suspensionYScaleValueDefaultMin;
    private readonly float _maxValue = 0.5f;
    private readonly Transform _wheelTransform;
    private readonly Transform _springTransform;
    private readonly Transform _cylinderTransform;
    private readonly Action<float> _calculateSound;
    private readonly JointSuspension2D _suspension;
    private float _previousPos;
    private float _differencePos;
    private Vector3 _currentScale;
    public Suspension(Transform spring, Transform insideCylinder,  WheelJoint2D joint,
        float suspensionStiffness, float suspensionYScaleValueDefaultMin, Action<float> calculateSound = null)
    {
        _suspension = joint.suspension;
        _suspension.frequency = suspensionStiffness;
        joint.suspension = _suspension;

        _wheelTransform = joint.connectedBody.transform;
        _previousPos = _wheelTransform.localPosition.y;

        _springTransform = spring;
        _cylinderTransform = insideCylinder;
        _suspensionYScaleValueDefaultMin = suspensionYScaleValueDefaultMin;
        _calculateSound = calculateSound;
    }

    public void Calculate()
    {
        _differencePos = _wheelTransform.localPosition.y - _previousPos;
        _previousPos = _wheelTransform.localPosition.y;

        _currentScale = _springTransform.localScale;
        _currentScale.y = _currentScale.y - _differencePos;
        _springTransform.localScale = _currentScale;
        _cylinderTransform.localScale = _currentScale;
        
        _calculateSound?.Invoke(Mathf.InverseLerp(_suspensionYScaleValueDefaultMin, _maxValue, _springTransform.localScale.y));
        
    }
}
