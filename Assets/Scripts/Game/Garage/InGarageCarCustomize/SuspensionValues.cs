using UnityEngine;

public class SuspensionValues : MonoBehaviour
{
    [SerializeField] private Transform _spring;
    [SerializeField] private Transform _insideCilinder;
    [SerializeField] private float _suspensionStiffness;

    public Suspension GetSuspension(WheelJoint2D frontWheelJoint)
    {
        return new Suspension(_spring, _insideCilinder, frontWheelJoint, _suspensionStiffness);
    }
}