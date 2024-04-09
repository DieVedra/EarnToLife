using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Box : DestructibleObject, IHitable
{
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private WoodDestructibleAudioHandler _woodDestructibleAudioHandler;
    public Vector2 Position => _transform.position;
    public bool IsBroken => base.ObjectIsBroken;
    public new IReadOnlyList<DebrisFragment> DebrisFragments => base.DebrisFragments;

    [Inject]
    private void Construct(LevelAudioHandler levelAudioHandler)
    {
        _woodDestructibleAudioHandler = levelAudioHandler.WoodDestructibleAudioHandler;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            result = TryDestruct(forceHit);
        }
        else
        {
            result = false;
        }
        // Debug.Log($"Box TryBreakOnImpact: {result}   forceHit: {forceHit} ");
        return result;
    }

    public void AddForce(Vector2 force)
    {
        _rigidbody2D.AddForce(force);
    }
    private new void OnEnable()
    {
        OnDestruct += _woodDestructibleAudioHandler.PlayWoodBreakingSound;
        OnDestructFail += _woodDestructibleAudioHandler.PlayWoodNotBreakingSound;
        OnDebrisHit += _woodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnEnable();
    }

    private new void OnDisable()
    {
        OnDestruct -= _woodDestructibleAudioHandler.PlayWoodBreakingSound;
        OnDestructFail -= _woodDestructibleAudioHandler.PlayWoodNotBreakingSound;
        OnDebrisHit -= _woodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnDisable();
    }
}