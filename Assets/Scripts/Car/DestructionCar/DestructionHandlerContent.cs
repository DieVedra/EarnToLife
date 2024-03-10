using UnityEngine;

public class DestructionHandlerContent
{
    public readonly Speedometer Speedometer;
    public readonly Transform DebrisParent;
    public readonly LayerMask CanCollisionsLayerMasks;
    public readonly int CarDebrisLayer;
    
    public DestructionHandlerContent(Speedometer speedometer, Transform debrisParent, LayerMask canCollisionsLayerMasks, int carDebrisLayer)
    {
        Speedometer = speedometer;
        DebrisParent = debrisParent;
        CanCollisionsLayerMasks = canCollisionsLayerMasks;
        CarDebrisLayer = carDebrisLayer;
    }
}