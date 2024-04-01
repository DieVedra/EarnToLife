using System;
using Cinemachine;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

public class LimitRideBack : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider2D;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Transform _pointContinuationPursuit;
    [SerializeField] private float _offset;
    private Camera _camera;
    private Transform _transform;
    private Transform _targetTransform;
    private float _previosPosX;
    private float _leftBorderPosX;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private bool _targetFall = false; 
    public void Init(bool key)
    {
        if (key == true)
        {
            _transform = transform;
            _targetTransform = _cinemachineVirtualCamera.Follow;
            _collider2D.gameObject.SetActive(true);
            SetColliderOffset();
            SetPreviousPosition();
            Observable.EveryUpdate().Subscribe(_ =>
            {
                TryMoveForward();
            }).AddTo(_compositeDisposable);
        }
        else
        {
            _collider2D.gameObject.SetActive(false);
        }
    }

    private void TryMoveForward()
    {
        if (_targetFall == false)
        {
            if (_previosPosX > _targetTransform.position.x)
            {
                _cinemachineVirtualCamera.Follow = null;
                _targetFall = true;
                return;
            }
            SetPreviousPosition();
        }
        else
        {
            if (_targetTransform.position.x > _pointContinuationPursuit.position.x)
            {
                _cinemachineVirtualCamera.Follow = _targetTransform;
                _targetFall = false;
            }
        }
    }

    private void SetPreviousPosition()
    {
        _previosPosX = _targetTransform.position.x;
    }
    private void SetColliderOffset()
    {
        _camera = Camera.main;
        Rect rect = _camera.pixelRect;
        Vector2 pos1 = _camera.ScreenToWorldPoint(rect.min);
        Vector2 pos2 = transform.InverseTransformPoint(pos1);
        float posX = pos2.x - _collider2D.size.x * 0.5f;
        _collider2D.offset = new Vector2 (posX, _collider2D.offset.y);
    }
    private void OnDisable()
    {
        _compositeDisposable.Dispose();
    }
}