using UnityEngine;

public interface IShotable
{
    public bool IsBroken { get; }
    public Transform TargetTransform { get; }
    public void DestructFromShoot(Vector2 force);
}
