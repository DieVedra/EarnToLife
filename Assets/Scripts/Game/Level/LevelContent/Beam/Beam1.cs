﻿using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Beam1 : Beam, IHitable, ICutable
{
    private float _halfBeamLength;
    private Transform _transform;
    public Vector2 Position => _transform.position;
    public bool IsBroken => ObjectIsBroken;
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

    [Inject]
    private void Construct(ILevel level, LevelAudioHandler levelAudioHandler)
    {
        DebrisParentForDestroy = level.DebrisParent;
        WoodDestructibleAudioHandler = levelAudioHandler.WoodDestructibleAudioHandler;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
        SetPositionsFragments();
        SetSizeToFragments();
    }
    public void DestructFromCut(Vector2 cutPos)
    {
        if (IsBroken == false)
        {
            WoodDestructibleAudioHandler.PlayWoodBreakingSound();
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
                WoodDestructibleAudioHandler.PlayWoodBreakingSound();
                Destruct();
                result = true;
            }
            else
            {
                WoodDestructibleAudioHandler.PlayWoodNotBreakingSound();
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    public void AddForce(Vector2 force)
    {
        Rigidbody2D.AddForce(force * ForceMultiplierWholeObject);
    }

    protected override void SetPositionsFragments()
    {
        SetPositionFragment(_spriteFragment1, GetFirstPositionFragment());
        SetPositionFragment(_spriteFragment2, GetEndPositionFragment());
    }

    protected override void SetSizeToFragments()
    {
        CalculateHalfBeamLength();
        SetSizeToFragment(_spriteFragment1, _halfBeamLength);
        SetSizeToFragment(_spriteFragment2, _halfBeamLength);
    }
    private void CalculateHalfBeamLength()
    {
        _halfBeamLength = _sprite.size.x * HalfWidthMultiplier  + _offsetSizeFragment;
    }

    private new void OnEnable()
    {
        OnDebrisHit += WoodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnEnable();
    }

    private new void OnDisable()
    {
        OnDebrisHit -= WoodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnDisable();
    }
}