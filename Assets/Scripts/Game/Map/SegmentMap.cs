using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SegmentMap : MonoBehaviour
{
    private const int SEGMENT_MULTIPLIER = 5;
    // private const float TIME = 1f;
    
     
    [SerializeField, Expandable] private MapPrefabsProvider _mapPrefabsProvider;
    [SerializeField] private Transform[] _points;
    [SerializeField] private Transform _dottedLineParent;
    [SerializeField] private DirectionSegment _directionSegment;
    [SerializeField, Range(0f,0.2f)] private float _step = 0.03f;

    private Spawner _spawner;
    private MapFlags _mapFlags;
    private MapLine _mapLine;
    // private List<Transform> _dots;
    public void Init(Spawner spawner)
    {
        _spawner = spawner;
        _mapLine = new MapLine(_points, _dottedLineParent, _directionSegment, _spawner, _mapPrefabsProvider, _step);
        _mapFlags = new MapFlags(_spawner, _mapPrefabsProvider);
    }

    public void Dispose()
    {
        _mapLine.Dispose();
        _mapFlags.Dispose();
    }

    public void CreateSegment(bool isLastSegment = false)
    {
        _mapLine.CreateLine(isLastSegment);
        _mapFlags.CreateFlags(_points[0], _points[^1], isLastSegment);
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            if (_points.Length >= 4)
            {
                int segmentsNumber = SEGMENT_MULTIPLIER * _points.Length;
                Vector3 previousPont = _points[0].position;
                for (int i = 0; i <= segmentsNumber; i++)
                {
                    float time = (float)i / segmentsNumber;
                    Vector3 newPoint = BezierPointCalculator.GetPoint(_points, time);
                    Gizmos.DrawLine(previousPont, newPoint);
                    previousPont = newPoint;
                }
            }
        }
    }
}
