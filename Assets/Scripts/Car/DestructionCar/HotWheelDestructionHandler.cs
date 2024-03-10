using UnityEngine;

public class HotWheelDestructionHandler
{
    public readonly HotWheelRef HotWheelRef;
    private readonly HotWheel _hotWheel;
    public HotWheelDestructionHandler(HotWheelRef hotWheelRef, HotWheel hotWheel)
    {
        HotWheelRef = hotWheelRef;
        _hotWheel = hotWheel;
    }

    public void Destruct()
    {
        _hotWheel.Dispose();
        HotWheelRef.Wheel1.gameObject.AddComponent<Rigidbody2D>();
        HotWheelRef.Wheel2.gameObject.AddComponent<Rigidbody2D>();
    }
}