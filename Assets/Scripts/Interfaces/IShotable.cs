using UnityEngine;

public interface IShotable
{
    // public bool IsLive { get; }
    public bool IsBroken { get; }
    public Transform TargetTransform { get; }
    public void DestructFromShoot(Vector2 force);
}
