using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Zombie : MonoBehaviour
{
    [SerializeField] private HingeJoint2D[] _hingeJoints2D;
    [SerializeField] private Collider2D[] _colliders2D;
    [SerializeField] private Rigidbody2D[] _rigidbodies2D;
    [SerializeField] private IKManager2D _ikManager;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        SwitchComponents(false);
    }
[Button("on")]
    private void a()
    {
        SwitchComponents(true);

    }
    private void SwitchComponents(bool key)
    {
        _ikManager.enabled = !key;
        _animator.enabled = !key;
        foreach (var joint2D in _hingeJoints2D)
        {
            joint2D.enabled = key;
        }

        foreach (var collider2D in _colliders2D)
        {
            collider2D.enabled = key;
        }

        foreach (var rigidbody2D in _rigidbodies2D)
        {
            rigidbody2D.simulated = key;
        }
    }
}
