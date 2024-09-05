using UnityEngine;

public class DestructionHandlerContent
{
    public int CarDebrisLayerNonInteractionWithCar;
    public int CarDebrisInteractingWithCar;
    public readonly Speedometer Speedometer;
    public readonly DebrisParent DebrisParent;
    public readonly LayerMask CanCollisionsLayerMasks;

    public DestructionHandlerContent(Speedometer speedometer, DebrisParent debrisParent, LayerMask canCollisionsLayerMasks,
        int carDebrisLayerNonInteractionWithCar, int carDebrisInteractingWithCar)
    {
        Speedometer = speedometer;
        DebrisParent = debrisParent;
        CanCollisionsLayerMasks = canCollisionsLayerMasks;
        CarDebrisLayerNonInteractionWithCar = carDebrisLayerNonInteractionWithCar;
        CarDebrisInteractingWithCar = carDebrisInteractingWithCar;
    }
}