using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Barrel : DestructibleObject, IHitable
{
    [SerializeField] private float _force;
    [SerializeField] private float _radiusShockWave;
    [SerializeField] private AnimationCurve _extinctionBlastWaveCurve;
    [SerializeField] private ParticleSystem _explodeEffect;
    private Rigidbody2D _rigidbody2D;
    private BlastWave _blastWave;
    private BarrelAudioHandler _barrelAudioHandler;
    public new IReadOnlyList<DebrisFragment> DebrisFragments => base.DebrisFragments;

    public Vector2 Position { get; }
    public bool IsBroken => base.ObjectIsBroken;

    [Inject]
    private void Construct(LevelAudioHandler levelAudioHandler)
    {
        _barrelAudioHandler = levelAudioHandler.BarrelAudioHandler;
        // _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _blastWave = new BlastWave(transform, _extinctionBlastWaveCurve, _radiusShockWave, _force);
    }

    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (TryDestruct(forceHit) == true)
            {
                Explosion();
                result = true;
            }
            else
            {
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
        OnDestruct += _barrelAudioHandler.PlayBarrelExplosionSound;
        OnDestructFail += _barrelAudioHandler.PlayBarrelFailBreakingSound;
        OnDebrisHit += _barrelAudioHandler.PlayBarrelHitSound;
        base.OnEnable();

    }

    private new void OnDisable()
    {
        OnDestruct -= _barrelAudioHandler.PlayBarrelExplosionSound;
        OnDestructFail -= _barrelAudioHandler.PlayBarrelFailBreakingSound;
        OnDebrisHit -= _barrelAudioHandler.PlayBarrelHitSound;
        base.OnDisable();

    }
}