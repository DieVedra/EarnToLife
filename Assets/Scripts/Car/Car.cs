using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Car : MonoBehaviour
{
    private readonly Vector2 _anchorJointOnCarBrokenIntoTwoParts = new Vector2(-2.82999992f, -1.85000002f);
    private readonly float _frequencyJointOnCarBrokenIntoTwoParts = 20f;
    [SerializeField, Foldout("WheelJoints"), HorizontalLine(color:EColor.Violet)] protected WheelJoint2D FrontWheelJoint;
    [SerializeField, Foldout("WheelJoints")] protected WheelJoint2D BackWheelJoint;
    protected Suspension FrontSuspension;
    protected Suspension BackSuspension;
    protected CustomizeCar CustomizeCar;
    protected WheelCarValues FrontWheelCarValues;
    protected WheelCarValues BackWheelCarValues;
    protected SuspensionValues _frontSupensionValues;
    protected SuspensionValues _backSuspensionValues;

    protected void ReinitBackWheelAndSuspensionOnCarBrokenIntoTwoParts(WheelJoint2D joint2D, WheelCarValues wheelCarValues)
    {
        wheelCarValues.gameObject.SetActive(true);
        InitWheelJoint(BackWheelJoint, wheelCarValues);
        JointSuspension2D suspension2D = BackWheelJoint.suspension;
        suspension2D.frequency = _frequencyJointOnCarBrokenIntoTwoParts;
        BackWheelJoint.suspension = suspension2D;
        joint2D.connectedBody = BackWheelCarValues.WheelRigidbody2D;
        joint2D.anchor = _anchorJointOnCarBrokenIntoTwoParts;
        InitSuspension(out BackSuspension, _backSuspensionValues, joint2D);
    }
    private void InitWheelsAndSuspension(IReadOnlyList<GameObject> parts)
    {
        ResetValues();
        List<WheelCarValues> wheelCarValues = new List<WheelCarValues>(2);
        List<SuspensionValues> suspensionValues = new List<SuspensionValues>(2);
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].TryGetComponent(out WheelCarValues wheelValue))
            {
                wheelCarValues.Add(wheelValue);
            }
            if (parts[i].TryGetComponent(out SuspensionValues suspensionValue))
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
        InitSuspension(out FrontSuspension, _frontSupensionValues, FrontWheelJoint);
    }
    private void InitBackSuspension()
    {
        InitSuspension(out BackSuspension, _backSuspensionValues, BackWheelJoint);
    }

    private void InitSuspension(out Suspension suspension, SuspensionValues suspensionValues, WheelJoint2D joint2D)
    {
        suspension = suspensionValues.GetSuspension(joint2D);
    }
    private void InitFrontWheelJoint()
    {
        InitWheelJoint(FrontWheelJoint, FrontWheelCarValues);

    }
    private void InitBackWheelJoint()
    {
        InitWheelJoint(BackWheelJoint, BackWheelCarValues);
    }

    private void InitWheelJoint(WheelJoint2D joint2D, WheelCarValues values)
    {
        joint2D.anchor = values.Anchor;
        joint2D.connectedBody = values.WheelRigidbody2D;
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
}
