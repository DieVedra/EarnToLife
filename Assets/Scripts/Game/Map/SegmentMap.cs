using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SegmentMap : MonoBehaviour
{
    private const int SEGMENT_MULTIPLIER = 5;
    private const float TIME = 1f;
    [SerializeField] private Transform[] _points;
    [SerializeField] private Transform _lineSpritePrefab;
    [SerializeField] private Transform _dottedLineParent;
    [SerializeField, Range(0f,0.2f)] private float _step = 0.03f;
    private List<SpriteRenderer> _spriteRenderers;
    public IReadOnlyList<Transform> Points => _points;
    public void Init(bool isLastSegment = true)
    {
        if (isLastSegment == false)
        {
            _points[_points.Length - 1].gameObject.SetActive(false);
        }
    }
    [ContextMenu("CreateLine")]
    private void CreateLine()
    {
        _spriteRenderers = new List<SpriteRenderer>();
        Vector3 newPoint = BezierPointCalculator.GetPoint(_points, _step);
        Vector3 nextPoint;
        for (float i = _step; i < TIME; i += _step)
        {
            nextPoint = BezierPointCalculator.GetPoint(_points, i += _step);
            Transform spriteTransform = Instantiate(_lineSpritePrefab, newPoint, Quaternion.identity, _dottedLineParent);
            spriteTransform.LookAt(nextPoint);
            newPoint = nextPoint;
            _spriteRenderers.Add(spriteTransform.GetComponentInChildren<SpriteRenderer>());
        }
    }
    private void OnDrawGizmos()
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
