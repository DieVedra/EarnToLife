using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
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
    protected SuspensionValues FrontSuspensionValues;
    protected SuspensionValues BackSuspensionValues;
    protected SuspensionAudioHandler SuspensionAudioHandler;
    protected void ReinitBackWheelAndSuspensionOnCarBrokenIntoTwoParts(WheelJoint2D joint2D, WheelCarValues wheelCarValues)
    {
        wheelCarValues.gameObject.SetActive(true);
        InitWheelJoint(BackWheelJoint, wheelCarValues);
        JointSuspension2D suspension2D = BackWheelJoint.suspension;
        suspension2D.frequency = _frequencyJointOnCarBrokenIntoTwoParts;
        BackWheelJoint.suspension = suspension2D;
        joint2D.connectedBody = BackWheelCarValues.WheelRigidbody2D;
        joint2D.anchor = _anchorJointOnCarBrokenIntoTwoParts;
        InitSuspension(out BackSuspension, BackSuspensionValues, joint2D);
    }

    protected void InitWheelsAndSuspension(IReadOnlyList<GameObject> parts)
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
        FrontSuspensionValues = suspensionValues[0];
        BackSuspensionValues = suspensionValues[1];
        InitFrontWheelJoint();
        InitBackWheelJoint();
        InitFrontSuspension();
        InitBackSuspension();
    }

    private void ResetValues()
    {
        FrontWheelCarValues = null;
        BackWheelCarValues = null;
        FrontSuspensionValues = null;
        BackSuspensionValues = null;
    }

    private void InitFrontSuspension()
    {
        if (SuspensionAudioHandler != null)
        {
            InitSuspension(out FrontSuspension, FrontSuspensionValues, FrontWheelJoint, SuspensionAudioHandler.CalculateVolumeFrontSuspension);
        }
        else
        {
            InitSuspension(out FrontSuspension, FrontSuspensionValues, FrontWheelJoint);
        }
    }
    private void InitBackSuspension()
    {
        if (SuspensionAudioHandler != null)
        {
            InitSuspension(out BackSuspension, BackSuspensionValues, BackWheelJoint, SuspensionAudioHandler.CalculateVolumeBackSuspension);
        }
        else
        {
            InitSuspension(out BackSuspension, BackSuspensionValues, BackWheelJoint);
        }
    }
    private void InitSuspension(out Suspension suspension, SuspensionValues suspensionValues, WheelJoint2D joint2D)
    {
        suspension = new Suspension(suspensionValues.Spring, suspensionValues.InsideCylinder, joint2D, 
            suspensionValues.SuspensionStiffness, suspensionValues.SuspensionYScaleValueDefault);
    }
    private void InitSuspension(out Suspension suspension, SuspensionValues suspensionValues, WheelJoint2D joint2D, Action<float> calculateSoundOperation)
    {
        suspension = new Suspension(suspensionValues.Spring, suspensionValues.InsideCylinder, joint2D, 
            suspensionValues.SuspensionStiffness, suspensionValues.SuspensionYScaleValueDefault, calculateSoundOperation);
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
}
