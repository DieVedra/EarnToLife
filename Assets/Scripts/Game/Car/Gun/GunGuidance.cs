using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGuidance
{
    private readonly float _rotateAddValue = 180f;
    
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
            _angleInDegrees = AngleCalculator.Calculate(target.position, _gunRotation.position);
            GunRotate();
        }
    }
    private void GunRotate()
    {
        _gunRotation.rotation = Quaternion.Lerp(
            _gunRotation.rotation, 
            Quaternion.Euler(_gunRotation.rotation.eulerAngles.x, _gunRotation.rotation.eulerAngles.y, _angleInDegrees + _rotateAddValue), 
            _speedLook * Time.deltaTime);
    }
}
