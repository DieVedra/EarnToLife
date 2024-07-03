using UnityEngine;

public static class Extensions
{
    
    public static Vector2 PositionVector2(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
    public static Vector2 LocalPositionVector2(this Transform transform)
    {
        return new Vector2(transform.localPosition.x, transform.localPosition.y);
    }
    public static Vector2 UpVector2(this Transform transform)
    {
        return new Vector2(transform.up.x, transform.up.y);
    }

    public static float RadiusCircle(this CapsuleCollider2D capsuleCollider2D)
    {
        return capsuleCollider2D.size.x * 0.5f;
    }
}