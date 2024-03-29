﻿using System;
using UnityEngine;

public class CabineDestructionHandler :DestructionHandler, IDispose
{
    private readonly CabineRef _cabineRef;
    private readonly Transform _headrest;
    private readonly Collider2D _helmetCollider;
    private readonly Collider2D _headrestCollider;
    private bool _isBroken = false;
    public event Action OnDriverCrushed;

    public CabineDestructionHandler(CabineRef cabineRef, DestructionHandlerContent destructionHandlerContent)
        : base(cabineRef, destructionHandlerContent)
    {
        _cabineRef = cabineRef;
        _headrest = cabineRef.Headrest;
        _helmetCollider = cabineRef.Helmet.GetComponent<Collider2D>();
        _headrestCollider = cabineRef.Headrest.GetComponent<Collider2D>();
        SubscribeCollider(_helmetCollider, CheckCollision, TryDriverDestruction);
        SubscribeCollider(_headrestCollider, CheckCollision, TryDriverDestruction);
    }

    public void TryDriverDestruction()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            CompositeDisposable.Clear();
            TryAddRigidBody(_headrest.gameObject);
            SetParentDebris(_headrest);
            OnDriverCrushed?.Invoke();
            Debug.Log("game over driver crushed");
        }
    }
    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
}