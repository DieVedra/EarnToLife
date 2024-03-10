using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ComponentRagdoll : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private HingeJoint2D _hingeJoint;
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;
    private Collider2D _collider;

    public Rigidbody2D Rigidbody => _rigidbody;
    public HingeJoint2D HingeJoint => _hingeJoint;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Transform TransformRagdoll => _transform;
    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_rigidbody.isKinematic = true;
        TryGetComponent(out HingeJoint2D _hingeJoint);
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer.gameObject.TryGetComponent(out Collider2D collider2D) == true)
        {
            _collider = collider2D;
        }
        else
        {
            _collider = _spriteRenderer.gameObject.GetComponentInChildren<Collider2D>();
        }
        _transform = transform;
    }
}
