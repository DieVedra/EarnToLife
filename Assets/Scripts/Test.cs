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
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D;
    [SerializeField] private ParticleSystem _particleFire;
    [SerializeField] private ParticleSystem _particleSmoke;
    [SerializeField] private float _sizeFragment;
    // [SerializeField,Range(-5f,5f)] private float _correction = 0f;
    
    [SerializeField] private float _sizePolygon;
    [SerializeField] private float _ltPolygon;
    
    [SerializeField] private float _sizeCircle;
    [SerializeField] private float _ltCircle;
    
    [SerializeField] private float _sizeBox;
    [SerializeField] private float _ltBox;
    
    [SerializeField] private float _sizeCapsule;
    [SerializeField] private float _ltCapsule;
    
    
    
    
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
    [Button("testpolygon")]
    private void a()
    {
        Vector2[] pathPoints = _polygonCollider2D.GetPath(0);
        float maxY = pathPoints.OrderByDescending(p => p.y).FirstOrDefault().y;
        float maxX = pathPoints.OrderByDescending(p => p.x).FirstOrDefault().x;
        float minY = pathPoints.OrderBy(p => p.y).FirstOrDefault().y;
        float minX = pathPoints.OrderBy(p => p.x).FirstOrDefault().x;
        _sizeFragment = Vector2.Distance(new Vector2(maxX, maxY), new Vector2(minX, minY));

        ParticleSystem.MainModule mainModule = _particleSmoke.main;

        mainModule.startSize = _sizeFragment + _sizePolygon;
        mainModule.startLifetime = _ltPolygon;

        
        _particleSmoke.transform.SetParent(_polygonCollider2D.transform);
        _particleSmoke.transform.position = _polygonCollider2D.transform.position;

        _particleSmoke.Play();
    }
    [Button("testcircle")]
    private void b()
    {
        _sizeFragment = _circleCollider2D.radius;

        ParticleSystem.MainModule mainModule = _particleSmoke.main;
        mainModule.startSize = _sizeFragment + _sizeCircle;
        mainModule.startLifetime = _ltCircle;

        _particleSmoke.transform.SetParent(_circleCollider2D.transform);
        _particleSmoke.transform.position = _circleCollider2D.transform.position;


        _particleSmoke.Play();
    }
    [Button("testbox")]
    private void c()
    {
        _sizeFragment = Mathf.Sqrt(_boxCollider2D.size.x * _boxCollider2D.size.x + _boxCollider2D.size.y * _boxCollider2D.size.y);
        ParticleSystem.MainModule mainModule = _particleSmoke.main;
        mainModule.startSize = _sizeFragment + _sizeBox;
        mainModule.startLifetime = _ltBox;

        
        _particleSmoke.transform.SetParent(_boxCollider2D.transform);
        _particleSmoke.transform.position = _boxCollider2D.transform.position;
        _particleSmoke.Play();
    }
    [Button("testcapsule")]
    private void d()
    {
        float height = _capsuleCollider2D.size.y - _capsuleCollider2D.size.x;
        _sizeFragment = Mathf.Sqrt(_capsuleCollider2D.size.x * _capsuleCollider2D.size.x + height * height);
        ParticleSystem.MainModule mainModule = _particleSmoke.main;

        mainModule.startSize = _sizeFragment + _sizeCapsule;
        mainModule.startLifetime = _ltCapsule;
        
        _particleSmoke.transform.SetParent(_capsuleCollider2D.transform);
        _particleSmoke.transform.position = _capsuleCollider2D.transform.position;
        _particleSmoke.Play();
    }
    [Button("stop")]
    private void stop()
    {
        _particleSmoke.Stop();
    }
}
