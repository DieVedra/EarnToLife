using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Coil : DestructibleObject, IHitable
{
    private Transform _transform;
    private WoodDestructibleAudioHandler _woodDestructibleAudioHandler;
    public Vector2 Position => _transform.position;
    public bool IsBroken => ObjectIsBroken;
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;
    
    [Inject]
    private void Construct(ILevel level, LevelAudioHandler levelAudioHandler)
    {
        DebrisParentForDestroy = level.DebrisParent;
        _woodDestructibleAudioHandler = levelAudioHandler.WoodDestructibleAudioHandler;
        Rigidbody2D = GetComponent<Rigidbody2D>();

        _transform = transform;
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
                _woodDestructibleAudioHandler.PlayWoodNotBreakingSound();
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
    private new void OnEnable()
    {
        OnDebrisHit += _woodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnEnable();
    }
    private new void OnDisable()
    {
        OnDebrisHit -= _woodDestructibleAudioHandler.PlayHitWoodSound;
        base.OnDisable();
    }
}
