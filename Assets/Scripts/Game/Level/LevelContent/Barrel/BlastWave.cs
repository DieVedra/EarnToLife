using System.Collections.Generic;
using UnityEngine;

public class BlastWave
{
    private readonly float _pointReference = 0f;
    private readonly float _force;
    private readonly float _radiusShockWave;
    private readonly float _radiusBurnWave;
    private readonly DebrisPool _debrisPoolEffects;
    private readonly Transform _transformPointReference;
    private readonly AnimationCurve _extinctionBlastWaveCurve;
    private readonly ContactFilter2D _contactFilter;
    private readonly List<Collider2D> _hitCollidersAfterSphereCast = new List<Collider2D>();
    private float _forceShockWaveInRadius;

    public BlastWave(DebrisPool debrisPool, Transform transformPointReference, AnimationCurve extinctionBlastWaveCurve, LayerMask blastWaveMask,
        float radiusShockWave, float radiusBurnWave, float force)
    {
        _debrisPoolEffects = debrisPool;
        _transformPointReference = transformPointReference;
        _extinctionBlastWaveCurve = extinctionBlastWaveCurve;
        _radiusShockWave = radiusShockWave;
        _radiusBurnWave = radiusBurnWave;
        _force = force;
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(blastWaveMask);
    }

    public void InteractWithBlastWave()
    {
        if (TryCastSphere())
        {
            for (int i = 0; i < _hitCollidersAfterSphereCast.Count; i++)
            {
                TryInteract(_hitCollidersAfterSphereCast[i]);
            }
        }
    }
    private void TryInteract(Collider2D collider2D)
    {
        // if (collider2D.TryGetComponent(out Rigidbody2D rigidbody2D))
        // {
        //     var forceDirection = CalculateDirectionBlastWave(rigidbody2D.position, _transformPointReference.position)
        //                 * CalculateForceShockWaveInRadius(rigidbody2D.position);
        //     rigidbody2D.AddForce(forceDirection);
        //     TryAddEffectToDebrisPiece(rigidbody2D.transform);
        // }
        if (collider2D.TryGetComponent(out DebrisFragment debrisFragment))
        {
            var forceDirection = CalculateDirectionBlastWave(debrisFragment.FragmentTransform.position, _transformPointReference.position)
                                 * CalculateForceShockWaveInRadius(debrisFragment.FragmentTransform.position);
            debrisFragment.Rigidbody2D.AddForce(forceDirection);
            TryAddEffectToDebrisPiece(debrisFragment);
        }
        else if (collider2D.transform.parent.TryGetComponent(out IExplosive explosive))
        {
            if (explosive.TryBreakOnExplosion(
                CalculateDirectionBlastWave(explosive.Position, _transformPointReference.position),
                CalculateForceShockWaveInRadius(explosive.Position)) == true)
            {
                AddForceBlastWaveToDebris(explosive.DebrisFragments);
            }
        }
    }
    private void AddForceBlastWaveToDebris(IReadOnlyList<DebrisFragment> debrisFragments)
    {
        for (int i = 0; i < debrisFragments.Count; i++)
        {
            debrisFragments[i].TryAddForce(
                CalculateDirectionBlastWave(debrisFragments[i].FragmentTransform.position, _transformPointReference.position)
                * CalculateForceShockWaveInRadius(debrisFragments[i].FragmentTransform.position));
            TryAddEffectToDebrisPiece(debrisFragments[i]);
        }
    }
    
    private void TryAddEffectToDebrisPiece(DebrisFragment debrisFragment)
    {
        float distance = CalculateDistance(debrisFragment.FragmentTransform.position);
        if (distance < _radiusBurnWave)
        {
            _debrisPoolEffects.GetDebrisBarrelEffect().PlayEffectTo(debrisFragment, CalculateIntensitySmoke(distance));
        }
    }
    // private void TryAddEffectToDebrisPiece(Transform transform)
    // {
    //     float distance = CalculateDistance(transform.position);
    //     if (distance < _radiusBurnWave)
    //     {
    //         _debrisPoolEffects.GetDebrisBarrelEffect().PlayEffectTo(transform, CalculateIntensitySmoke(distance));
    //     }
    // }
    private bool TryCastSphere()
    {
        if (Physics2D.OverlapCircle(_transformPointReference.position, _radiusShockWave, _contactFilter, _hitCollidersAfterSphereCast) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private float CalculateForceShockWaveInRadius(Vector2 positionHit)
    {
        float distance = CalculateDistance(positionHit);
        float valueForCurve = Mathf.InverseLerp(_radiusShockWave, _pointReference, distance);
        float forceMultiplier = _extinctionBlastWaveCurve.Evaluate(valueForCurve);
        float forceShockWave = _force * forceMultiplier;
        return forceShockWave;
    }

    private float CalculateDistance(Vector2 positionHit)
    {
        return Vector2.Distance(_transformPointReference.position, positionHit);
    }
    private float CalculateIntensitySmoke(float distance)
    {
        return Mathf.InverseLerp(_radiusBurnWave, _pointReference, distance);
    }
    private Vector2 CalculateDirectionBlastWave(Vector2 hitPoint, Vector2 referencePoint)
    {
        return hitPoint - referencePoint;
    }
}