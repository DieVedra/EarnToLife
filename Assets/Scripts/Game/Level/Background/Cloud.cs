
using UnityEngine;

public class Cloud
{
    private readonly Vector2 _speedAddedRange;
    private readonly Vector2 _scaleMultiplier = new Vector2(0.14f,5.22f);
    private readonly Vector3 _rotationVar1 = new Vector3(0f,0f,-90f);
    private readonly Vector3 _rotationVar2 = new Vector3(0f,0f,90f);
    private readonly Transform _transform;
    private readonly Transform _leftBorder;
    private readonly Transform _rightBorder;
    private readonly CloudShape[] _cloudShapes;
    private int _customizeIndex;
    private float _additiveSpeed;
    private Vector2 _newPosition;

    public Cloud(Transform transform, Transform leftBorder, Transform rightBorder, Vector2 speedAddedRange)
    {
        _speedAddedRange = speedAddedRange;
        _transform = transform;
        _leftBorder = leftBorder;
        _rightBorder = rightBorder;
        var newScale = transform.localScale * _scaleMultiplier;
        _cloudShapes = new[]
        {
            new CloudShape(_transform.localScale, Vector3.zero), 
            new CloudShape(newScale, _rotationVar1), 
            new CloudShape(newScale, _rotationVar2), 
        };
        _customizeIndex = UnityEngine.Random.Range(0,_cloudShapes.Length);
        RandomCustomize();
    }
    private void RandomCustomize()
    {
        _additiveSpeed = Random.Range(_speedAddedRange.x, _speedAddedRange.y);
        _transform.localScale = _cloudShapes[_customizeIndex].Scale;
        _transform.rotation = Quaternion.Euler(_cloudShapes[_customizeIndex].Rotation);
        if (_customizeIndex == _cloudShapes.Length - 1)
        {
            _customizeIndex = 0;
        }
        else
        {
            _customizeIndex++;
        }
    }

    private void MoveToStartPosition(ref Vector2 position)
    {
        _transform.position = position;
        RandomCustomize();
    }
    public void Move(float speed)
    {
        if (_transform.position.x > _rightBorder.position.x)
        {
            _newPosition.x = _leftBorder.position.x;
            _newPosition.y = _transform.position.y;
            MoveToStartPosition(ref _newPosition);
        }
        else if (_transform.position.x < _leftBorder.position.x)
        {
            _newPosition.x = _rightBorder.position.x;
            _newPosition.y = _transform.position.y;
            MoveToStartPosition(ref _newPosition);
        }
        else
        {
            _newPosition.x = speed + _additiveSpeed + _transform.LocalPositionVector2().x;
            _newPosition.y = _transform.LocalPositionVector2().y;
            _transform.localPosition = _newPosition;
        }
    }
}