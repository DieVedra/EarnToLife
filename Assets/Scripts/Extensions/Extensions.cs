using UnityEngine;

public static class Extensions
{
    public static Vector2 PositionVector2(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public static Vector2 UpVector2(this Transform transform)
    {
        return new Vector2(transform.up.x, transform.up.y);
    }
    public static Vector2 OriginLowerCirclePosition(this CapsuleCollider2D capsuleCollider2D, Transform transform)
    {
        var positionOffset = transform.PositionVector2() + capsuleCollider2D.offset;
        var posY =(capsuleCollider2D.size.y * 0.5f) - capsuleCollider2D.RadiusCircle();
        return  new Vector2(positionOffset.x, positionOffset.y - posY);
    }

    public static float RadiusCircle(this CapsuleCollider2D capsuleCollider2D)
    {
        return capsuleCollider2D.size.x * 0.5f;
    }
}