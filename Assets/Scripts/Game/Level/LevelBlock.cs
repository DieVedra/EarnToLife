using System.Collections.Generic;
using NaughtyAttributes;
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

    private List<ActivityObject> _activityObjects;
    private List<List<ActivityObject>> _activityObjectsSeparated;
    private bool _isBlockContentEmpty => _activityObjects.Count > 0 ? false : true;
    public float PointEnableNextBlockX => _pointEnableNextBlock.position.x;
    public float PointDisablePreviousBlockX => _pointDisablePreviousBlock.position.x;

    public void Init(ActivityHandler activityHandler, LevelBlockContentGrouper levelBlockContentGrouper)
    {
        _activityHandler = activityHandler;
        _activityObjects = levelBlockContentGrouper.GetActivityContent(_content);
        if (_isBlockContentEmpty == false)
        {
            _activityObjectsSeparated = _arraySeparator.Separate(_activityObjects);
        }
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        if (gameObject.activeSelf == false)
        {
            Debug.Log($"Activate LevelBlock");
            gameObject.SetActive(true);
            if (_isBlockContentEmpty == false)
            {
                _frameByFrameDivider.FrameByFrameSeparatedOperation(true,
                    () => { _activityHandler.UpdateActivityFromList(_activityObjectsSeparated[0]);
                    },
                    () =>
                    {
                        _activityHandler.UpdateActivityFromList(_activityObjectsSeparated[1]);
                    },
                    () =>
                    {
                        _activityHandler.UpdateActivityFromList(_activityObjectsSeparated[2]);
                    });
            }
        }
    }

    public void Deactivate()
    {
        if (gameObject.activeSelf == true)
        {
            Debug.Log($"Deactivate LevelBlock");

            _frameByFrameDivider.Dispose();
            gameObject.SetActive(false);
        }
    }
}