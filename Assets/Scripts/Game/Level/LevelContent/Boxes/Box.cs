using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Box : DestructibleObject, IHitable, ICutable
{
    private readonly float _forceMultiplier = 3f;
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private WoodDestructibleAudioHandler _woodDestructibleAudioHandler;
    public Vector2 Position => _transform.position;
    public bool IsBroken => base.ObjectIsBroken;
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

    [Inject]
    private void Construct(LevelAudioHandler levelAudioHandler)
    {
        _woodDestructibleAudioHandler = levelAudioHandler.WoodDestructibleAudioHandler;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    public void DestructFromCut()
    {
        if (ObjectIsBroken == false)
        {
            _woodDestructibleAudioHandler.PlayWoodBreakingSound();
            Destruct();
        }
        Debug.Log($"Cut");
    }

    public bool TryBreakOnImpact(float forceHit)
    {
        Debug.Log($"Hit");

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
        _rigidbody2D.AddForce(force * _forceMultiplier);
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