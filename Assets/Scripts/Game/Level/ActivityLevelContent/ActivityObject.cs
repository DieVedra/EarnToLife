
using UnityEngine;

public class ActivityObject
{
    public readonly GameObject GameObject;
    public readonly float StartRangeValue;
    public readonly float EndRangeValue;

    public ActivityObject(Transform transform, float addRangeValue)
    {
        GameObject = transform.gameObject;
        GameObject.SetActive(false);
        var position = transform.position;
        StartRangeValue = position.x - addRangeValue;
        EndRangeValue = position.x + addRangeValue;
    }
}