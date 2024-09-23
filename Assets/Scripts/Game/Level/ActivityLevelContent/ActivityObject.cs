
using UnityEngine;

[System.Serializable]
public class ActivityObject
{
    public GameObject GameObject1;

    public readonly GameObject GameObject;
    public readonly float StartRangeValue;
    public readonly float EndRangeValue;

    public ActivityObject(Transform transform, float addRangeValue)
    {
        GameObject = transform.gameObject;
        GameObject1 = GameObject;
        GameObject.SetActive(false);
        var position = transform.position;
        StartRangeValue = position.x - addRangeValue;
        EndRangeValue = position.x + addRangeValue;
    }
}