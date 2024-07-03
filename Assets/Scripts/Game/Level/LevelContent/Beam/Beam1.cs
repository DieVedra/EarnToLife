using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Beam1 : Beam, IHitable, ICutable
{
    private float _halfBeamLength;
    public Vector2 Position => TransformBase.position;
    public bool IsBroken => ObjectIsBroken;

    private void Awake()
    {
        SetPositionsFragments();
        SetSizeToFragments();
    }
    public void DestructFromCut(Vector2 cutPos)
    {
        if (NotDestructible == true)
        {
            WoodDestructibleAudioHandler.PlayWoodBreakingSound();
        }
        else
        {
            if (IsBroken == false)
            {
                WoodDestructibleAudioHandler.PlayWoodBreakingSound();
                Destruct();
            }
        }
    }
    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if (NotDestructible == true)
        {
            WoodDestructibleAudioHandler.PlayWoodNotBreakingSound(forceHit);
            result = false;
        }
        else
        {
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
                    WoodDestructibleAudioHandler.PlayWoodNotBreakingSound(forceHit);
                    result = false;
                }
            }
            else
            {
                result = false;
            }
        }

        return result;
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
        base.OnEnable();
    }
}