
using System;
using NaughtyAttributes;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [InfoBox("PointEnableNextBlock", EInfoBoxType.Normal)]
    [SerializeField] private Transform _pointEnableNextBlock;
    [InfoBox("PointDisablePreviousBlock", EInfoBoxType.Normal)]
    [SerializeField] private Transform _pointDisablePreviousBlock;
    
    public float PointEnableNextBlockX => _pointEnableNextBlock.position.x;
    public float PointDisablePreviousBlockX => _pointDisablePreviousBlock.position.x;

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}