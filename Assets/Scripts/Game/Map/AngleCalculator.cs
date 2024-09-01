
using UnityEngine;

public static class AngleCalculator
{
    public static float Calculate(Vector2 point1, Vector2 point2)
    {
        Vector2 pointsDifference = point2 - point1;
        return Mathf.Atan2(pointsDifference.y, pointsDifference.x) * Mathf.Rad2Deg;
    }
        
}