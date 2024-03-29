using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGuidance
{
    private Transform _gunRotation;
    private Vector3 _targetPosition;
    private Vector3 _gunPosition;
    private Vector2 _difference;
    private float _angleInRadians;
    private float _angleInDegrees;
    private float _speedLook;
    public bool IsGuidanence { get; set; } = true;
    public GunGuidance(Transform gunRotation, float speedLook)
    {
        _gunRotation = gunRotation;
        _speedLook = speedLook;
    }
    public void Update(Transform target)
    {
        if (IsGuidanence == true)
        {
            CalculateRotation(target.position);
            GunRotate(_angleInDegrees);
        }
    }
    private void CalculateRotation(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _gunPosition = _gunRotation.position;
        _difference = _targetPosition - _gunPosition;


        _angleInRadians = Mathf.Atan2(_difference.y, _difference.x);
        _angleInDegrees = _angleInRadians * Mathf.Rad2Deg;
    }
    private void GunRotate(float angle)
    {
        _gunRotation.rotation = Quaternion.Lerp(_gunRotation.rotation, Quaternion.Euler(_gunRotation.rotation.eulerAngles.x, _gunRotation.rotation.eulerAngles.y, angle), _speedLook * Time.deltaTime);
    }
}
