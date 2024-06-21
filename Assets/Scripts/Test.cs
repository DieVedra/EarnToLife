using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Vector2 _previousPos;



    [Button("11")]
    private void a()
    {
        _previousPos = _transform.position;
    }
    private void CalculateSpeedX()
    {
        Vector2 pos = _transform.PositionVector2();
        Vector2 displacement = pos - _previousPos;
        float speed = displacement.magnitude / Time.deltaTime;
        Debug.Log($"   {_transform.PositionVector2()}  {_previousPos}   {displacement}  {displacement.magnitude}    {speed}");
        _previousPos = _transform.PositionVector2();

        // return speed;
    }
    
    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
    }

    private void Update()
    {
        CalculateSpeedX();
    }
}
