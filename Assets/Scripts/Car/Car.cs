using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField, Foldout("WheelJoints"), HorizontalLine(color:EColor.Violet)] protected WheelJoint2D FrontWheelJoint;
    [SerializeField, Foldout("WheelJoints")] protected WheelJoint2D BackWheelJoint;
    protected Suspension FrontSuspension;
    protected Suspension BackSuspension;
    protected CustomizeCar CustomizeCar;


    protected WheelCarValues FrontWheelCarValues;
    protected WheelCarValues BackWheelCarValues;
    protected SuspensionValues _frontSupensionValues;
    protected SuspensionValues _backSuspensionValues;
    
    private void InitWheelsAndSuspension(IReadOnlyList<GameObject> parts)
    {
        ResetValues();
        List<WheelCarValues> wheelCarValues = new List<WheelCarValues>(2);
        List<SuspensionValues> suspensionValues = new List<SuspensionValues>(2);
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].TryGetComponent<WheelCarValues>(out WheelCarValues wheelValue))
            {
                wheelCarValues.Add(wheelValue);
            }
            if (parts[i].TryGetComponent<SuspensionValues>(out SuspensionValues suspensionValue))
            {
                suspensionValues.Add(suspensionValue);
            }
        }
        FrontWheelCarValues = wheelCarValues[0];
        BackWheelCarValues = wheelCarValues[1];
        _frontSupensionValues = suspensionValues[0];
        _backSuspensionValues = suspensionValues[1];
        InitFrontWheelJoint();
        InitBackWheelJoint();
        InitFrontSuspension();
        InitBackSuspension();
    }

    private void ResetValues()
    {
        FrontWheelCarValues = null;
        BackWheelCarValues = null;
        _frontSupensionValues = null;
        _backSuspensionValues = null;
    }
    private void InitFrontSuspension()
    {
        FrontSuspension = _frontSupensionValues.GetSuspension(FrontWheelJoint);
    }
    private void InitBackSuspension()
    {
        BackSuspension = _backSuspensionValues.GetSuspension(BackWheelJoint);
    }
    private void InitFrontWheelJoint()
    {
        FrontWheelJoint.anchor = FrontWheelCarValues.Anchor;
        FrontWheelJoint.connectedBody = FrontWheelCarValues.WheelRigidbody2D;
    }
    private void InitBackWheelJoint()
    {
        BackWheelJoint.anchor = BackWheelCarValues.Anchor;
        BackWheelJoint.connectedBody = BackWheelCarValues.WheelRigidbody2D;
    }
    private void OnEnable()
    {
        CustomizeCar ??= GetComponent<CustomizeCar>();
        CustomizeCar.OnSetWheels += InitWheelsAndSuspension;
    }

    private void OnDisable()
    {
        CustomizeCar.OnSetWheels -= InitWheelsAndSuspension;
    }
    //standartwhels
    //Vector2(2.82131338,-0.143588513)    back   Vector2(-1.85021126,-0.144302696)
    //midwhells
    //Vector2(2.82422614,-0.231972486)   back   Vector2(-1.85621881,-0.24492754)
    //max
    //Vector2(2.83800006,-0.530967772) back  Vector2(-1.847,-0.600000024)
}
