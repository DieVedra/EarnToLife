using UnityEngine;

public struct CloudShape
{
    public readonly Vector2 Scale;
    public readonly Vector3 Rotation;

    public CloudShape(Vector2 scale, Vector3 rotation)
    {
        Scale = scale;
        Rotation = rotation;
    }
}