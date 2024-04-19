using System;
using UnityEngine;

public class GlassDestructionHandler : DestructionHandler, IDispose
{
    private readonly GlassRef _glassRef;
    private readonly DestructionEffectsHandler _destructionEffectsHandler;
    private Transform _glassNormal;
    private Transform _glassDamaged;
    private Transform _currentGlass;
    private bool _isBreaked = false;
    private bool _isBroken = false;
    public GlassDestructionHandler(GlassRef glassRef, DestructionHandlerContent destructionHandlerContent, DestructionEffectsHandler destructionEffectsHandler)
    :base(glassRef, destructionHandlerContent, " GlassDestruction ", maxStrength: glassRef.StrengthGlass)
    {
        TryInitGlasses(glassRef);
        _glassRef = glassRef;
        _destructionEffectsHandler = destructionEffectsHandler;
        SubscribeCollider(_glassNormal.GetComponent<Collider2D>(), CheckCollision, TryBreakGlassFromHit);
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
            TryBreakGlassFromHit();
            CompositeDisposable.Clear();
            TryAddRigidBody(_currentGlass.gameObject);
            SetParentDebris();
            SetCarDebrisLayerInteractableWithCar();
        }
    }

    public void TryBreakGlassFromWings()
    {
        TryBreakGlass(_glassRef.transform.position, ImpulseNormalValue);
    }

    private void TryBreakGlassFromHit()
    {
        TryBreakGlass(HitPosition, ImpulseNormalValue);
    }
    private void TryBreakGlass(Vector2 position, float force)
    {
        if (_isBreaked == false)
        {
            _isBreaked = true;
            _destructionEffectsHandler.HitBrokenEffect(position, force);
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