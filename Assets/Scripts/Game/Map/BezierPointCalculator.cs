using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierPointCalculator
{
    public static Vector3 GetPoint(Transform[] points, float t)
    {
        Vector3[] pointsPos = new Vector3[points.Length];
        for (int i = 0; i < pointsPos.Length; i++)
        {
            pointsPos[i] = points[i].position;
        }
        Vector3[] intermediatePoints = CalculateIntermediatePoints(pointsPos, t);
        for (int i = 0; i < points.Length - 2; i++)
        {
            intermediatePoints = CalculateIntermediatePoints(intermediatePoints, t);
        }
        return intermediatePoints[0];
    }
    private static Vector3[] CalculateIntermediatePoints(Vector3[] points, float t)
    {
        Vector3[] intermediatePoints = new Vector3[points.Length - 1];

        for (int i = 0; i < intermediatePoints.Length; i++)
        {
            intermediatePoints[i] = Vector3.Lerp(points[i], points[i + 1], t);
        }
        return intermediatePoints;
    }

}
