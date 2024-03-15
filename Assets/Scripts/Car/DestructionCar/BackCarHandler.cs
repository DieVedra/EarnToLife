using System;
using UnityEngine;

public class BackCarHandler
{
    private readonly BackWingDestructionHandler _backWingDestructionHandler;
    private readonly WheelCarValues _invisibleWheel;
    private readonly Transform _backCar;
    private WheelJoint2D _wheelJoint2D;
    public event Action<WheelJoint2D, WheelCarValues> OnCarBrokenIntoTwoParts; 
    public BackCarHandler(BottomRef bottomRef, BackWingDestructionHandler backWingDestructionHandler)
    {
        _backWingDestructionHandler = backWingDestructionHandler;
        _backCar = bottomRef.BackCar;
        _invisibleWheel = bottomRef.InvisibleWheel;
        _invisibleWheel.gameObject.SetActive(false);
    }

    public void DestructNow()
    {
        _backCar.gameObject.AddComponent<WheelJoint2D>();
        _wheelJoint2D = _backCar.gameObject.GetComponent<WheelJoint2D>();
        OnCarBrokenIntoTwoParts?.Invoke(_wheelJoint2D, _invisibleWheel);
    }
}