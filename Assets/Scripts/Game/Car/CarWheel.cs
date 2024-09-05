using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel
{
    private readonly Transform _transformThisWheel;
    private readonly Transform _wheelsTransform;
    private readonly CircleCollider2D _circleCollider2D;
    private readonly ParticleSystem _smokeWheelParticleSystem;
    private readonly ParticleSystem _dirtWheelParticleSystem;
    private readonly ParticleSystem _bloodWheelParticleSystem;
    public Vector3 Position => _transformThisWheel.position;
    public float Radius => _wheelsTransform.localScale.y * _circleCollider2D.radius;
    public Collider2D Collider2D => _circleCollider2D;
    public ParticleSystem SmokeWheelParticleSystem => _smokeWheelParticleSystem;
    public ParticleSystem DirtWheelParticleSystem => _dirtWheelParticleSystem;
    public ParticleSystem BloodWheelParticleSystem => _bloodWheelParticleSystem;

    public CarWheel(WheelJoint2D wheelJoint, Rigidbody2D rigidbody2D, Transform wheelsTransform,
        ParticleSystem smokeWheelParticleSystem, ParticleSystem dirtWheelParticleSystem, ParticleSystem bloodWheelParticleSystem)
    {
        _wheelsTransform = wheelsTransform;
        _transformThisWheel = wheelJoint.connectedBody.gameObject.transform;
        _circleCollider2D = rigidbody2D.GetComponent<CircleCollider2D>();
        
        _smokeWheelParticleSystem = smokeWheelParticleSystem;
        _dirtWheelParticleSystem = dirtWheelParticleSystem;
        _bloodWheelParticleSystem = bloodWheelParticleSystem;
        _smokeWheelParticleSystem.gameObject.SetActive(true);
        _dirtWheelParticleSystem.gameObject.SetActive(true);
        _bloodWheelParticleSystem.gameObject.SetActive(true);
    }
}
