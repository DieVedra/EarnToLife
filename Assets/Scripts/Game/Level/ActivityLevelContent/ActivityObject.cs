
using UnityEngine;

[System.Serializable]
public class ActivityObject
{
    public readonly float StartXRangeValue;
    public readonly float EndXRangeValue;
    public readonly float StartYRangeValue;
    public readonly float EndYRangeValue;
    public readonly GameObject GameObject;

    public ActivityObject(Transform transform, float addXRangeValue, float addYRangeValue)
    {
        GameObject = transform.gameObject;
        GameObject.SetActive(false);
        var position = transform.position;
        StartXRangeValue = position.x - addXRangeValue;
        EndXRangeValue = position.x + addXRangeValue;
        StartYRangeValue = position.y + addYRangeValue;
        EndYRangeValue = position.y - addYRangeValue;
    }
}