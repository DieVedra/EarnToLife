using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGuidance
{
    private Transform _gunRotation;
    private Vector2 _difference;
    private float _angleInRadians;
    private float _angleInDegrees;
    private float _speedLook;
    public bool FreezedGuidanenceAfterShoot { get; set; } = false;
    public GunGuidance(Transform gunRotation, float speedLook)
    {
        _gunRotation = gunRotation;
        _speedLook = speedLook;
    }
    public void Update(Transform target)
    {
        if (FreezedGuidanenceAfterShoot == false)
        {
            // CalculateRotation(target.position);
            _angleInDegrees = AngleCalculator.Calculate(target.position, _gunRotation.position);
            GunRotate(_angleInDegrees);
        }
    }
    // private void CalculateRotation(Vector3 targetPosition)
    // {
    //     _difference = targetPosition - _gunRotation.position;
    //
    //
    //     _angleInRadians = Mathf.Atan2(_difference.y, _difference.x);
    //     _angleInDegrees = _angleInRadians * Mathf.Rad2Deg;
    // }
    private void GunRotate(float angle)
    {
        _gunRotation.rotation = Quaternion.Lerp(_gunRotation.rotation, Quaternion.Euler(_gunRotation.rotation.eulerAngles.x, _gunRotation.rotation.eulerAngles.y, angle), _speedLook * Time.deltaTime);
    }
}
