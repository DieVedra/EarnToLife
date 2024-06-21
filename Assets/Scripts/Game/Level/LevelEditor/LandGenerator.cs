using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class LandGenerator : MonoBehaviour
{
    [SerializeField] private bool _isOn;
    [SerializeField] private SpriteShapeController _spriteShapeController;

    [SerializeField, Range(0.1f, 200.0f)] private int _levelLenght;
    [SerializeField, Range(1f, 50f)] private float _xMultiplier;
    [SerializeField, Range(1f, 50f)] private float _yMultiplier;
    [SerializeField, Range(0f, 1f)] private float _curveSmoothness;

    [SerializeField, Range(0.1f, 1f)] private float _noiseStep = 0.5f;
    [SerializeField, Range(1f, 10f)] private float _bottom = 10f;

    private Vector3 _lastPos;
    
    [SerializeField] private List<Vector3> _positions;
    
    [Button("Save")]
    private void Save()
    {
        _positions = new List<Vector3>(_spriteShapeController.spline.GetPointCount());
        int count = _spriteShapeController.spline.GetPointCount();
        
        for (int i = 0; i < count; i++)
        {
            _positions.Add(_spriteShapeController.spline.GetPosition(i));
        }
    }
    private void OnValidate()
    {
        if (_isOn == true)
        {
            _spriteShapeController.spline.Clear();
            for (int i = 0; i < _levelLenght; i++)
            {
                _lastPos = Vector3.zero +

                           new Vector3(i * _xMultiplier, Mathf.PerlinNoise(0, i * _noiseStep) * _yMultiplier);
                _spriteShapeController.spline.InsertPointAt(i, _lastPos);
                if (i != 0 && i != _levelLenght - 1)
                {
                    _spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                    _spriteShapeController.spline.SetLeftTangent(i, Vector3.left * _xMultiplier * _curveSmoothness);
                    _spriteShapeController.spline.SetRightTangent(i, Vector3.right * _xMultiplier * _curveSmoothness);
                }

            }
            _spriteShapeController.spline.InsertPointAt(_levelLenght,
                new Vector3(_lastPos.x, Vector3.zero.y - _bottom));
            _spriteShapeController.spline.InsertPointAt(_levelLenght + 1,
                new Vector3(Vector3.zero.x, Vector3.zero.y - _bottom));
        }
    }
}
