using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt
{
    private const float UP_BOARD_VALUE = 45f;
    private const float DOWN_BOARD_VALUE = 135f;
    private const float ROTATE_COMPENSATING_VALUE = 90f;
    private const float BODY_ROTATE_COMPENSATING_VALUE = 90f;
    private const float GUN_ROTATE_COMPENSATING_VALUE = 180f;
    private const float LEFT_DOWN_ANGLE = -128f;
    private const float LEFT_UP_ANGLE = 130f;
    private const float RIGHT_DOWN_ANGLE = -50f;
    private const float RIGHT_UP_ANGLE = 50f;

    private Flip _flipBody;
    private Vision _vision;
    private Transform _gun;
    private Transform _head;
    private Transform _body;

    private Vector2 _targetPosition;
    private Vector2 _gunPosition;
    private Vector2 _difference;
    private float _angleInRadians;
    private float _angleInDegrees;
    private float _speedLook;
    public LookAt(Flip flipBody, Vision vision, Transform gun, Transform head, Transform body, float speedLook)
    {
        _body = body;
        _head = head;
        _flipBody = flipBody;
        _vision = vision;
        _gun = gun;
        _speedLook = speedLook;
    }
    public void Update()
    {
        CalculateRotation();
        SetRotation();
    }
    private void CalculateRotation()
    {
        _targetPosition = _vision.Target;
        _gunPosition = _gun.position;
        _difference = _targetPosition - _gunPosition;


        _angleInRadians = Mathf.Atan2(_difference.y, _difference.x);
        _angleInDegrees = _angleInRadians * Mathf.Rad2Deg;
    }
    private void SetRotation()
    {
        HeadRotate(_angleInDegrees - ROTATE_COMPENSATING_VALUE);

        if (_flipBody.IsFliped == true)
        {
            GunRotate(_angleInDegrees);

            if (_angleInDegrees < RIGHT_DOWN_ANGLE)
            {
                BodyRotate(_angleInDegrees - UP_BOARD_VALUE);
            }
            else if (_angleInDegrees > RIGHT_UP_ANGLE)
            {
                BodyRotate(_angleInDegrees - DOWN_BOARD_VALUE);
            }
            else
            {
                BodyRotate(-BODY_ROTATE_COMPENSATING_VALUE);
            }
        }
        else
        {
            GunRotate(_angleInDegrees + GUN_ROTATE_COMPENSATING_VALUE);

            if (_angleInDegrees > 0f && _angleInDegrees < LEFT_UP_ANGLE)
            {
                BodyRotate(_angleInDegrees - UP_BOARD_VALUE);
            }
            else if (_angleInDegrees < 0f && _angleInDegrees > LEFT_DOWN_ANGLE)
            {
                BodyRotate(_angleInDegrees - DOWN_BOARD_VALUE);
            }
            else
            {
                BodyRotate(BODY_ROTATE_COMPENSATING_VALUE);
            }
        }
    }
    private void GunRotate(float angle)
    {
        _gun.rotation = Quaternion.Lerp(_gun.rotation, Quaternion.Euler(_gun.rotation.eulerAngles.x, _gun.rotation.eulerAngles.y, angle), _speedLook * Time.deltaTime);
    }
    private void BodyRotate(float angle)
    {
        _body.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    private void HeadRotate(float angle)
    {
        _head.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
