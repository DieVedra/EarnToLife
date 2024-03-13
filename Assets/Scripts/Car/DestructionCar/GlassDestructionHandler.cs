﻿using UnityEngine;

public class GlassDestructionHandler : DestructionHandler, IDispose
{
    private Transform _glassNormal;
    private Transform _glassDamaged;
    private Transform _currentGlass;
    private bool _isBreaked = false;
    private bool _isBroken = false;
    public GlassDestructionHandler(GlassRef glassRef, DestructionHandlerContent destructionHandlerContent)
    :base(glassRef, destructionHandlerContent, glassRef.StrengthGlass)
    {
        TryInitGlasses(glassRef);
        SubscribeCollider(_glassNormal.GetComponent<Collider2D>(), CheckCollision, TryBreakGlass);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }

    public void TryThrowGlass()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            TryBreakGlass();
            CompositeDisposable.Clear();
            TryAddRigidBody(_currentGlass.gameObject);
            SetParentDebris();
            SetCarDebrisLayer();
            
        }
    }

    public void TryBreakGlass()
    {
        if (_isBreaked == false)
        {
            _isBreaked = true;
            CompositeDisposable.Clear();
            TrySwitchSprites();
        }
    }

    private void TrySwitchSprites()
    {
        if (_glassDamaged != null)
        {
            _glassNormal.gameObject.SetActive(false);
            _glassDamaged.gameObject.SetActive(true);
            _currentGlass = _glassDamaged;
        }
    }
    private void TryInitGlasses(GlassRef glassRef)
    {
        _glassNormal = glassRef.Glasses[0];
        _currentGlass = _glassNormal;
        if (glassRef.Glasses.Length > 1)
        {
            _glassDamaged = glassRef.Glasses[1];
        }
    }
}