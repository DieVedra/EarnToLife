
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class DebrisKeeper : MonoBehaviour
{
    [SerializeField] private float _addXRange = 30f;
    
    private Transform _transform;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private Transform _cameraTransform;
    
    private List<Transform> _debris = new List<Transform>(50);

    public void Init(Transform cameraTransform)
    {
        _transform = transform;
        _cameraTransform = cameraTransform;
    }

    public void DebrisCheckActivity()
    {
        for (int i = 0; i < _debris.Count; i++)
        {
            if (_debris[i].gameObject.activeSelf == true)
            {
                if (_debris[i].position.x  + _addXRange < _cameraTransform.position.x)
                {
                    _debris[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void Dispose()
    {
        _compositeDisposable.Clear();
    }

    public void AddDebris(Transform debrisTransform)
    {
        debrisTransform.SetParent(_transform);
        _debris.Add(debrisTransform);
    }
}