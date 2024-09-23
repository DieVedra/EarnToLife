using UnityEngine;

public class DestructionHandlerContent
{
    public readonly int CarDebrisLayerNonInteractionWithCar;
    public readonly int CarDebrisInteractingWithCar;
    public readonly Speedometer Speedometer;
    public readonly DebrisKeeper debrisKeeper;
    public readonly LayerMask CanCollisionsLayerMasks;

    public DestructionHandlerContent(Speedometer speedometer, DebrisKeeper debrisKeeper, LayerMask canCollisionsLayerMasks,
        int carDebrisLayerNonInteractionWithCar, int carDebrisInteractingWithCar)
    {
        Speedometer = speedometer;
        this.debrisKeeper = debrisKeeper;
        CanCollisionsLayerMasks = canCollisionsLayerMasks;
        CarDebrisLayerNonInteractionWithCar = carDebrisLayerNonInteractionWithCar;
        CarDebrisInteractingWithCar = carDebrisInteractingWithCar;
    }
}