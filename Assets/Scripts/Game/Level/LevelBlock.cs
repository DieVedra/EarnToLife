
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [SerializeField] private Transform _content;

    [InfoBox("PointEnableNextBlock", EInfoBoxType.Normal)]
    [SerializeField] private Transform _pointEnableNextBlock;

    [InfoBox("PointDisablePreviousBlock", EInfoBoxType.Normal)]
    [SerializeField] private Transform _pointDisablePreviousBlock;

    private readonly FrameByFrameDivider _frameByFrameDivider = new FrameByFrameDivider();
    private readonly ArraySeparator<ActivityObject> _arraySeparator = new ArraySeparator<ActivityObject>();
    private ActivityHandler _activityHandler;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();

    private List<ActivityObject> _activityObjects;
    private List<List<ActivityObject>> _activityObjectsSeparated;
    public float PointEnableNextBlockX => _pointEnableNextBlock.position.x;
    public float PointDisablePreviousBlockX => _pointDisablePreviousBlock.position.x;

    public void Init(ActivityHandler activityHandler, LevelBlockContentGrouper levelBlockContentGrouper)
    {
        _activityHandler = activityHandler;
        _activityObjects = levelBlockContentGrouper.GetActivityContent(_content);
        _activityObjectsSeparated = _arraySeparator.Separate(_activityObjects);
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        Debug.Log($"Activate");

        gameObject.SetActive(true);
        _frameByFrameDivider.FrameByFrameSeparatedOperation(true,
            () => { _activityHandler.UpdateActivityFromList(_activityObjectsSeparated[0]);
                Debug.Log($"UpdateActivityFromList 1");
            },
            () =>
            {
                _activityHandler.UpdateActivityFromList(_activityObjectsSeparated[1]);
                
                Debug.Log($"UpdateActivityFromList 2");
            },
            () =>
            {
                _activityHandler.UpdateActivityFromList(_activityObjectsSeparated[2]);
                Debug.Log($"UpdateActivityFromList 3");
            });
    }

    public void Deactivate()
    {
        // _compositeDisposable.Clear();
        _activityHandler.Dispose();
        gameObject.SetActive(false);
    }
}