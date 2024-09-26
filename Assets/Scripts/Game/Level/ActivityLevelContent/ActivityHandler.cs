using System.Collections.Generic;
using UnityEngine;

public class ActivityHandler
{
    private readonly Transform _cameraTransform;

    public ActivityHandler(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }
    public void UpdateActivityFromList(IReadOnlyList<ActivityObject> activityObjects)
    {
        foreach (ActivityObject activityObject in activityObjects)
        {
            if (activityObject.EndXRangeValue < _cameraTransform.position.x)
            {
                activityObject.GameObject.SetActive(false);
            }
            else if(activityObject.StartXRangeValue < _cameraTransform.position.x)
            {
                if (activityObject.StartYRangeValue > _cameraTransform.position.y && activityObject.EndYRangeValue < _cameraTransform.position.y)
                {
                    activityObject.GameObject.SetActive(true);
                }
            }
        }
    }
}