using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip
{
    private const float FLIPVALUE = -1f;

    private Transform _target;
    private Transform _flippableBody;
    private Vision _vision;
    private Vector3 _flipResult;
    private Vector3 _flipCurrentValue;
    private bool _isFliped;
    public bool IsFliped => _isFliped;
    public Flip(Transform target, Transform flippable, Vision vision)
    {
        _target = target;
        _flippableBody = flippable;
        _vision = vision;
        _isFliped = false;
    }

    public void CheckFlip()
    {
        if (_vision.TargetDetected == false)
        {
            return;
        }
        if (_target.position.x > _flippableBody.position.x)
        {
            if (_isFliped == true)
            {
                return;
            }
            DoFlip();
            _isFliped = true;
        }
        else
        {
            if (_isFliped == false)
            {
                return;
            }
            DoFlip();
            _isFliped = false;
        }
    }
    private void DoFlip()
    {
        _flipCurrentValue = _flippableBody.localScale;
        _flipResult = new Vector3(_flipCurrentValue.x * FLIPVALUE, _flipCurrentValue.y, _flipCurrentValue.z);
        _flippableBody.localScale = _flipResult;
    }
}
