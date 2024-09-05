using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MapFlags
{
    private readonly float _durationAnimation = 0.8f;
    private readonly float _endValueAnimation = 1.1f;
    private readonly Transform _currentPointFlag;
    private readonly Transform _previousPointFlag;
    private readonly Transform _nextPointFlag;
    private readonly Spawner _spawner;
    private List<Transform> _flags;
    private CancellationTokenSource _cancellationTokenSource;
    public MapFlags(Spawner spawner, MapPrefabsProvider mapPrefabsProvider)
    {
        _currentPointFlag = mapPrefabsProvider.CurrentPointFlagPrefab;
        _previousPointFlag = mapPrefabsProvider.PreviousPointFlagPrefab;
        _nextPointFlag = mapPrefabsProvider.NextPointFlagPrefab;
        _spawner = spawner;
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
    }
    public void CreateFlags(Transform point1, Transform point2, bool isLastSegment)
    {
        DestroyFlags();
        _flags = new List<Transform>(2);
        if (isLastSegment == true)
        {
            Create(_currentPointFlag, point1);
            NextPointFlagAnimation(Create(_nextPointFlag, point2));
        }
        else
        {
            Create(_previousPointFlag, point1);
        }
    }

    private void DestroyFlags()
    {
        if (_flags != null)
        {
            foreach (var flag in _flags)
            {
                if (flag != null)
                {
                    _spawner.KillObject(flag.gameObject);
                }
            }
        }
    }

    private Transform Create(Transform prefab, Transform point)
    {
        Transform flag = _spawner.Spawn(prefab, point, point);
        _flags.Add(flag);
        return flag;
    }

    private void NextPointFlagAnimation(Transform flag)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        flag.DOScale(_endValueAnimation, _durationAnimation).SetLoops(-1, LoopType.Yoyo).WithCancellation(_cancellationTokenSource.Token); 
    }
}