using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SegmentMap : MonoBehaviour
{
    private const int SEGMENT_MULTIPLIER = 5;
    private const float TIME = 1f;
    
     
    [SerializeField, Expandable] private MapPrefabsProvider _mapPrefabsProvider;
    [SerializeField] private Transform[] _points;
    [SerializeField] private Transform _dottedLineParent;
    [SerializeField] private DirectionSegment _directionSegment;
    [SerializeField, Range(0f,0.2f)] private float _step = 0.03f;

    private Spawner _spawner;
    private MapFlags _mapFlags;
    [SerializeField] private List<Transform> _dots;
    private bool _isLastSegment = false;
    public void Init(Spawner spawner, bool isLastSegment = false)
    {
        _spawner = spawner;
        _isLastSegment = isLastSegment;
        if (_mapFlags == null)
        {
            _mapFlags = new MapFlags(_spawner, _mapPrefabsProvider);
        }

        Dispose();
        CreateLine();
        _mapFlags.CreateFlags(_points[0], _points[^1], isLastSegment);
        gameObject.SetActive(true);
    }

    private void Dispose()
    {
        ClearLine();
        _mapFlags.DestroyFlags();
    }
    [Button("CreateLine")]
    private void CreateLine()
    {
        ClearLine();
        Vector3 newPoint = BezierPointCalculator.GetPoint(_points, _step);
        Vector3 nextPoint;
        for (float i = _step; i < TIME; i += _step)
        {
            nextPoint = BezierPointCalculator.GetPoint(_points, i += _step);
            Transform lineFragment = _spawner.Spawn(_mapPrefabsProvider.LineSpritePrefab, newPoint, _dottedLineParent);
            lineFragment.eulerAngles = new Vector3(0f,0f,AngleCalculator.Calculate(lineFragment.PositionVector2(), nextPoint) + (float)_directionSegment);
            newPoint = nextPoint;
            _dots.Add(lineFragment);
        }
    }

    [Button("ClearLine")]
    private void ClearLine()
    {
        if (_dots != null && _dots.Count > 0)
        {
            for (int i = 0; i < _dots.Count; i++)
            {
                if (_dots[i] != null)
                {
                    _spawner.KillObject(_dots[i].gameObject);
                }
            }
            _dots = new List<Transform>();
        }
    }
    private void OnDrawGizmos()
    {
        if (_points.Length >= 4)
        {
            int segmentsNumber = SEGMENT_MULTIPLIER * _points.Length;
            Vector3 previosPont = _points[0].position;
            for (int i = 0; i <= segmentsNumber; i++)
            {
                float time = (float)i / segmentsNumber;
                Vector3 newPoint = BezierPointCalculator.GetPoint(_points, time);
                Gizmos.DrawLine(previosPont, newPoint);
                previosPont = newPoint;
            }
        }
    }
}
