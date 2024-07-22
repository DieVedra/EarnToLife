
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class DebrisParent : MonoBehaviour
{
    private readonly float _addXRange = 30f;
    private Transform _transform;
    private List<Transform> _debris = new List<Transform>(50);
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private Transform _cameraTransform;
    [Inject]
    private void Construct(ILevel level)
    {
        _cameraTransform = level.CameraTransform;
    }
    private void Awake()
    {
        _transform = transform;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            for (int i = 0; i < _debris.Count; i++)
            {
                if (_debris[i].gameObject.activeSelf == true)
                {
                    if (_debris[i].position.x  + _addXRange < _cameraTransform.position.x)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }).AddTo(_compositeDisposable);
    }

    public void AddToDebris(Transform debrisTransform)
    {
        debrisTransform.SetParent(_transform);
        _debris.Add(debrisTransform);
    }

    private void OnDestroy()
    {
        _compositeDisposable.Clear();
    }
}