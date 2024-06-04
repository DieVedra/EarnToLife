using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ZombieMove
{
    private readonly float _dotValue = -0.03f;
    private readonly float _distance = 0.05f;
    private readonly float _radiusSphere;
    private readonly Vector2 _offsetSphere;
    private readonly float _directionMultiplier;
    private readonly Vector2 _direction = new Vector2(1f,1f);
    private readonly Transform _transform;
    private readonly Rigidbody2D _rigidbody2D;
    private readonly CapsuleCollider2D _capsuleCollider2D;
    private readonly GamePause _gamePause;
    private readonly float _speed;
    private float _dotNormal;
    private List<RaycastHit2D> _hits = new List<RaycastHit2D>();
    private ContactFilter2D _contactFilter;
    private Vector2 _normal => _hits[0].normal;
    public ZombieMove(Transform transform, Rigidbody2D rigidbody2D, CapsuleCollider2D capsuleCollider2D, 
        GamePause gamePause, LayerMask contactMask, Vector2 offsetSphere, float direction, float speed, float radiusSphere)
    {
        _transform = transform;
        _rigidbody2D = rigidbody2D;
        _capsuleCollider2D = capsuleCollider2D;
        _gamePause = gamePause;
        _directionMultiplier = direction;
        _speed = speed;
        _radiusSphere = radiusSphere;
        _offsetSphere = offsetSphere;
        _contactFilter.useTriggers = false;
        _contactFilter.useLayerMask = true;
        _contactFilter.layerMask = contactMask.value;
    }

    public void Update()
    {
        if (_gamePause.IsPause == false)
        {
            if (Physics2D.CircleCast(_transform.PositionVector2() + _offsetSphere, _radiusSphere,
                _direction, _contactFilter, _hits, _distance) > 0)
            {
                // Debug.Log($"Hit true  {result}    {_hits.Count}");
                // for (int i = 0; i < _hits.Count; i++)
                // {
                //     Debug.Log($"Hit true  {_hits[i].collider.transform.parent.name}  {_hits[i].point}     {_hits[i].normal}     {_transform.TransformPoint(_hits[i].normal)}");
                //     Debug.DrawLine(_hits[i].point, _hits[i].point + _hits[i].normal, Color.cyan);
                //     Debug.DrawLine(_hits[i].point, _hits[i].point + DirectionAlongNormal(), Color.green);
                //     
                // }
                _dotNormal = Vector2.Dot(_normal, Vector2.right);
                if (_dotNormal < _dotValue)
                {
                    _rigidbody2D.isKinematic = false;
                    Move((Vector2.right * _directionMultiplier) * _speed * Time.deltaTime);
                }
                else
                {
                    _rigidbody2D.isKinematic = true;
                    Move(DirectionAlongNormal() * _speed * Time.deltaTime);
                }
            }
            else
            {
                _rigidbody2D.isKinematic = false;
            }
        }
    }
    private Vector2 DirectionAlongNormal()
    {
        return Vector3.Cross(_normal, _transform.forward) * _directionMultiplier;
    }
    private void Move(Vector2 offset)
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + offset);
    }
}