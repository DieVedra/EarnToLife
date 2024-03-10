using UnityEngine;

public class Suspension
{
    private Transform _wheelTransform;

    private Transform _springTransform;
    private Transform _cylinderTransform;
    private JointSuspension2D _suspension;
    private float _previosPos;
    private float _differencePos;
    private Vector3 _currentScale;

    public Suspension(Transform spring, Transform insideCylinder,  WheelJoint2D joint, float suspensionStiffness)
    {
        _suspension = joint.suspension;
        _suspension.frequency = suspensionStiffness;
        joint.suspension = _suspension;

        _wheelTransform = joint.connectedBody.transform;
        _previosPos = _wheelTransform.localPosition.y;

        _springTransform = spring;
        _cylinderTransform = insideCylinder;
    }

    public void Calculate()
    {
        _differencePos = _wheelTransform.localPosition.y - _previosPos;
        _previosPos = _wheelTransform.localPosition.y;

        _currentScale = _springTransform.localScale;
        _currentScale.y = _currentScale.y - _differencePos;
        _springTransform.localScale = _currentScale;
        _cylinderTransform.localScale = _currentScale;

    }
}
