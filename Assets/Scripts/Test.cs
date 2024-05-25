using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
// [ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _polygonCollider2D;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _sizeFragment;
    [SerializeField,Range(-5f,5f)] private float _correction = 0f;
    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
    }

    private void Update()
    {
        // Debug.Log($"{_normal.up}     {_normal.forward}    {cross}");
        // Debug.DrawLine(_normal.position, _normal.position + _normal.up, Color.green);
        // Debug.DrawLine(_normal.position, _normal.position + cross, Color.magenta);
    }
    [Button("test")]
    private void CalculateSizePolygonCollider()
    {
        _sizeFragment = _circleCollider2D.radius;

        ParticleSystem.MainModule mainModule = _particle.main;

        mainModule.startSize = _sizeFragment + _correction;
        _particle.Play();
    }
    // private void CalculateSizePolygonCollider()
    // {
    //     Vector2[] pathPoints = _polygonCollider2D.GetPath(0);
    //     float maxY = pathPoints.OrderByDescending(p => p.y).FirstOrDefault().y;
    //     float maxX = pathPoints.OrderByDescending(p => p.x).FirstOrDefault().x;
    //
    //     float minY = pathPoints.OrderBy(p => p.y).FirstOrDefault().y;
    //     float minX = pathPoints.OrderBy(p => p.x).FirstOrDefault().x;
    //     _sizeFragment = Vector2.Distance(new Vector2(maxX, maxY), new Vector2(minX, minY));
    //
    //
    //
    //     ParticleSystem.MainModule mainModule = _particle.main;
    //
    //     mainModule.startSize = _sizeFragment + _correction;
    //     _particle.Play();
    // }
    [Button("stop")]
    private void stop()
    {
        _particle.Stop();
    }
}
