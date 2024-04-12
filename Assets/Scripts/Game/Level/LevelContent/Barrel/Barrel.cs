using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Barrel : DestructibleObject, IHitable, IShotable
{
    [SerializeField] private float _force;
    [SerializeField] private float _radiusShockWave;
    [SerializeField] private AnimationCurve _extinctionBlastWaveCurve;
    [SerializeField] private ParticleSystem _explodeEffect;
    private Rigidbody2D _rigidbody2D;
    private BlastWave _blastWave;
    private BarrelAudioHandler _barrelAudioHandler;
    private Transform _transform;
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

    public Vector2 Position => _transform.position;
    public bool IsBroken => base.ObjectIsBroken;

    public bool IsLive => base.ObjectIsBroken ?  false : true ;
    public Transform TargetTransform => _transform;

    [Inject]
    private void Construct(LevelAudioHandler levelAudioHandler)
    {
        _barrelAudioHandler = levelAudioHandler.BarrelAudioHandler;
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _blastWave = new BlastWave(transform, _extinctionBlastWaveCurve, _radiusShockWave, _force);
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
                _barrelAudioHandler.PlayBarrelExplosionSound();
                Destruct();
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
        _rigidbody2D.AddForce(force);
    }

    [Button("Explosion")]
    private void a()
    {
        TryBreakOnImpact(100);
    }

    private void Explosion()
    {
        _blastWave.InteractWithBlastWave(DebrisFragments);
        // _explodeEffect?.Play();
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radiusShockWave);
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