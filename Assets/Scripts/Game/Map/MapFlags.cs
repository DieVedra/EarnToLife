
using System.Collections.Generic;
using UnityEngine;

public class MapFlags
{
    private readonly Transform _currentPointFlag;
    private readonly Transform _previousPointFlag;
    private readonly Transform _nextPointFlag;
    private readonly Spawner _spawner;
    private List<Transform> _flags;
    public MapFlags(Spawner spawner, MapPrefabsProvider mapPrefabsProvider)
    {
        _currentPointFlag = mapPrefabsProvider.CurrentPointFlagPrefab;
        _previousPointFlag = mapPrefabsProvider.PreviousPointFlagPrefab;
        _nextPointFlag = mapPrefabsProvider.NextPointFlagPrefab;
        _spawner = spawner;
    }

    public void CreateFlags(Transform point1, Transform point2, bool isLastSegment)
    {
        _flags = new List<Transform>(2);
        if (isLastSegment == true)
        {
            Create(_currentPointFlag, point1);
            Create(_nextPointFlag, point2);
        }
        else
        {
            Create(_previousPointFlag, point1);
        }
    }

    public void DestroyFlags()
    {
        if (_flags != null && _flags.Count > 0)
        {
            Debug.Log($"_flags.Count {_flags.Count}");
            foreach (var flag in _flags)
            {
                if (flag != null)
                {
                    _spawner.KillObject(flag.gameObject);
                }
            }
        }
    }

    private void Create(Transform prefab, Transform point)
    {
        Transform flag = _spawner.Spawn(prefab, point, point);
        _flags.Add(flag);
    }
}