using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel
{
    private Transform _transformThisWheel;
    private Transform _wheelsTransform;
    private CircleCollider2D _circleCollider2D;
    public Vector3 Position => _transformThisWheel.position;
    public float Radius => _wheelsTransform.localScale.y * _circleCollider2D.radius;

    public CarWheel(WheelJoint2D wheelJoint, Rigidbody2D rigidbody2D, Transform wheelsTransform)
    {
        _wheelsTransform = wheelsTransform;
        _transformThisWheel = wheelJoint.connectedBody.gameObject.transform;
        _circleCollider2D = rigidbody2D.GetComponent<CircleCollider2D>();
    }
}
