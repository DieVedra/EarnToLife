using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _normal;
    private Vector3 pos;

    private RaycastHit2D _frontWheelHit;

    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(_origin.position, _normal.position);
        // Gizmos.color = Color.yellow;
        //
        // Gizmos.DrawLine(_origin.position, Vector3.Cross(_normal.position, Vector3.up));
        
    }

    private void Update()
    {

        var cross = Vector3.Cross(_normal.up, _normal.forward) * -1f;
        Debug.Log($"{_normal.up}     {_normal.forward}    {cross}");
        Debug.DrawLine(_normal.position, _normal.position + _normal.up, Color.green);
        Debug.DrawLine(_normal.position, _normal.position + cross, Color.magenta);
    }
}
