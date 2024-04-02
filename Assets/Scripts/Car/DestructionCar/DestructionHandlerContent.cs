using UnityEngine;

public class DestructionHandlerContent
{
    public int CarDebrisLayerNonInteractionWithCar;
    public int CarDebrisInteractingWithCar;
    public readonly Speedometer Speedometer;
    public readonly Transform DebrisParent;
    public readonly LayerMask CanCollisionsLayerMasks;

    public DestructionHandlerContent(Speedometer speedometer, Transform debrisParent, LayerMask canCollisionsLayerMasks,
        int carDebrisLayerNonInteractionWithCar, int carDebrisInteractingWithCar)
    {
        Speedometer = speedometer;
        DebrisParent = debrisParent;
        CanCollisionsLayerMasks = canCollisionsLayerMasks;
        CarDebrisLayerNonInteractionWithCar = carDebrisLayerNonInteractionWithCar;
        CarDebrisInteractingWithCar = carDebrisInteractingWithCar;
    }
}