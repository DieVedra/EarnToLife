using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Barrel : DestructibleObject, IHitable, IShotable, ICutable
{
    [SerializeField] private float _force;
    [SerializeField] private float _radiusShockWave;
    [SerializeField] private float _radiusBurnWave;
    [SerializeField] private LayerMask _blastWaveMask;
    [SerializeField] private AnimationCurve _extinctionBlastWaveCurve;
    private BarrelPool _barrelPoolEffects;
    private BlastWave _blastWave;
    private BarrelAudioHandler _barrelAudioHandler;
    private Transform _transform;
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

    public Vector2 Position => _transform.position;
    public bool IsBroken => base.ObjectIsBroken;

    public bool IsLive => base.ObjectIsBroken ?  false : true ;
    public Transform TargetTransform => _transform;

    [Inject]
    private void Construct(ILevel level)
    {
        _barrelPoolEffects = level.BarrelPool;
        DebrisParentForDestroy = level.DebrisParent;
        _barrelAudioHandler = level.LevelAudio.BarrelAudioHandler;
        _transform = transform;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _blastWave = new BlastWave(level.BarrelPool, WholeObjectTransform, _extinctionBlastWaveCurve, _blastWaveMask,
            _radiusShockWave, _radiusBurnWave, _force);
    }
    public void DestructFromCut(Vector2 cutPos)
    {
        if (ObjectIsBroken == false)
        {
            Destruct();
            Explosion();
        }
    }

    public void DestructFromShoot(Vector2 force)
    {
        if (IsLive == true)
        {
            Destruct();
            Explosion();
        }
    }

    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (forceHit > Hardness)
            {
                Destruct();
                Explosion();
                result = true;
            }
            else
            {
                _barrelAudioHandler.PlayBarrelFailBreakingSound();
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
        Rigidbody2D.AddForce(force);
    }
    private void Explosion()
    {
        _barrelAudioHandler.PlayBarrelExplosionSound();
        _blastWave.InteractWithBlastWave();
        _barrelPoolEffects.PlayBarrelExplosionEffect(WholeObjectTransform.position).Forget();
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(WholeObjectTransform.position, _radiusShockWave);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(WholeObjectTransform.position, _radiusBurnWave);
        }
    }
    private new void OnEnable()
    {
        OnDebrisHit += _barrelAudioHandler.PlayBarrelHitSound;
        base.OnEnable();
    }
    private new void OnDisable()
    {
        OnDebrisHit -= _barrelAudioHandler.PlayBarrelHitSound;
        base.OnDisable();
    }
}