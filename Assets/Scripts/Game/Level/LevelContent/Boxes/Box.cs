using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Box : DestructibleObject, IHitable, ICutable, IExplosive
{
    private readonly float _forceMultiplier = 3f;
    private Transform _transform;
    private WoodDestructibleAudioHandler _woodDestructibleAudioHandler;

    public Vector2 Position => _transform.position;

    public bool IsBroken => base.ObjectIsBroken;

    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

    [Inject]
    private void Construct(ILevel level)
    {
        DebrisParentForDestroy = level.DebrisParent;
        _woodDestructibleAudioHandler = level.LevelAudio.WoodDestructibleAudioHandler;
        DebrisHitSound = level.LevelAudio.WoodDestructibleAudioHandler.PlayHitWoodSound;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    public bool TryBreakOnExplosion(Vector2 direction, float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (forceHit > Hardness)
            {
                _woodDestructibleAudioHandler.PlayWoodBreakingSound();
                Destruct();
                result = true;
            }
            else
            {
                Rigidbody2D.AddForce(direction * forceHit * _forceMultiplier);
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    public void DestructFromCut(Vector2 cutPos)
    {
        if (ObjectIsBroken == false)
        {
            _woodDestructibleAudioHandler.PlayWoodBreakingSound();
            Destruct();
        }
    }

    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (forceHit > Hardness)
            {
                _woodDestructibleAudioHandler.PlayWoodBreakingSound();
                Destruct();
                result = true;
            }
            else
            {
                _woodDestructibleAudioHandler.PlayWoodNotBreakingSound(forceHit);
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    private new void OnEnable()
    {
        // debrisHitSound += _woodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnEnable();
    }

    private new void OnDisable()
    {
        // debrisHitSound -= _woodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnDisable();
    }
}