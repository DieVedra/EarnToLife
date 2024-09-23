using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MapLine
{
    private const float TIME = 1f;
    private const float FADE_DURATION = 0.8f;
    private const float MIN_VALUE_FADE = 0.2f;
    private const float DELAY_DURATION = 0.2f;
    private readonly Transform _lineSpritePrefab;
    private readonly Transform _dottedLineParent;
    private readonly DirectionSegment _directionSegment;
    private readonly Spawner _spawner;
    private readonly float _step;
    
    private readonly Transform[] _points;
    private List<Transform> _dots;
    private CancellationTokenSource _cancellationTokenSource;

    public MapLine(Transform[] points, Transform dottedLineParent, DirectionSegment directionSegment, Spawner spawner, MapPrefabsProvider mapPrefabsProvider, float step)
    {
        _points = points;
        _dottedLineParent = dottedLineParent;
        _directionSegment = directionSegment;
        _spawner = spawner;
        _lineSpritePrefab = mapPrefabsProvider.LineSpritePrefab;
        _step = step;
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
    }
    public void CreateLine(bool isLastSegment)
    {
        ClearLine();
        _dots = new List<Transform>();
        Vector3 newPoint = BezierPointCalculator.GetPoint(_points, _step);
        Vector3 nextPoint;
        for (float i = _step; i < TIME; i += _step)
        {
            nextPoint = BezierPointCalculator.GetPoint(_points, i += _step);
            Transform lineFragment = _spawner.Spawn(_lineSpritePrefab, newPoint, _dottedLineParent);
            lineFragment.eulerAngles = new Vector3(0f,0f,AngleCalculator.Calculate(lineFragment.PositionVector2(), nextPoint) + (float)_directionSegment);
            newPoint = nextPoint;
            _dots.Add(lineFragment);
        }
        if (isLastSegment == true)
        {
            PulseLineAnimation(_dots).Forget();
        }
    }

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
        }
    }

    private async UniTaskVoid PulseLineAnimation(IReadOnlyList<Transform> dots)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        foreach (var dot in dots)
        {
            dot.GetComponent<SpriteRenderer>().DOFade(MIN_VALUE_FADE, FADE_DURATION).SetLoops(-1, LoopType.Yoyo)
                .WithCancellation(_cancellationTokenSource.Token).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(DELAY_DURATION), cancellationToken: _cancellationTokenSource.Token);
        }
    }
}