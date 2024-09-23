using System.Collections.Generic;
using UnityEngine;

public class ActivityHandler
{
    private readonly Transform _cameraTransform;
    private readonly FrameByFrameDivider _frameByFrameDivider = new FrameByFrameDivider();

    public ActivityHandler(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    public void Dispose()
    {
        _frameByFrameDivider.Dispose();
    }
    public void UpdateActivityFromList(IReadOnlyList<ActivityObject> activityObjects)
    {
        foreach (ActivityObject activityObject in activityObjects)
        {
            if (activityObject.EndRangeValue < _cameraTransform.position.x)
            {
                activityObject.GameObject.SetActive(false);
            }
            else if(activityObject.StartRangeValue < _cameraTransform.position.x)
            {
                activityObject.GameObject.SetActive(true);
            }
        }
    }

    // public void CheckActivity(ActivityObject activityObject)
    // {
    //     if (activityObject.EndRangeValue < _cameraTransform.position.x)
    //     {
    //         activityObject.GameObject.SetActive(false);
    //     }
    //     else if(activityObject.StartRangeValue < _cameraTransform.position.x)
    //     {
    //         activityObject.GameObject.SetActive(true);
    //     }
    // }
}