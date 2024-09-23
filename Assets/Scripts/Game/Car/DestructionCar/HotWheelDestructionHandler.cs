using UnityEngine;

public class HotWheelDestructionHandler : DestructionHandler
{
    public readonly HotWheelRef HotWheelRef;
    private readonly HotWheel _hotWheel;

    public HotWheelDestructionHandler(HotWheelRef hotWheelRef, HotWheel hotWheel, DestructionHandlerContent destructionHandlerContent)
        : base(hotWheelRef, destructionHandlerContent)
    {
        HotWheelRef = hotWheelRef;
        _hotWheel = hotWheel;
    }

    public void Destruct()
    {
        _hotWheel.Destruct();
        SetParentDebris();
        SetCarDebrisLayerNonInteractableWithCar();
    }
}