using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IShotable
{
    public bool IsLive { get; private set; }
    public Transform TargetTransform { get; private set; }
    public void DestructFromShoot(Vector2 force)
    {
        Debug.Log("Looh");
    }

    private void Start()
    {
        TargetTransform = transform;
        IsLive = true;
    }
}
