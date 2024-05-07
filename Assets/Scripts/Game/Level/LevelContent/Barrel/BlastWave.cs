using System.Collections.Generic;
using UnityEngine;

public class BlastWave
{
    private readonly float _pointReference = 0f;
    private readonly float _force;
    private readonly float _radiusShockWave;
    private readonly float _radiusBurnWave;
    private readonly BarrelPool _barrelPool;
    private readonly Transform _transformPointReference;
    private readonly AnimationCurve _extinctionBlastWaveCurve;
    private readonly ContactFilter2D _contactFilter;
    private float _forceShockWaveInRadius;
    private List<Collider2D> _hitCollidersAfterSphereCast = new List<Collider2D>(200);
    public BlastWave(BarrelPool barrelPool, Transform transformPointReference, AnimationCurve extinctionBlastWaveCurve, LayerMask blastWaveMask,
        float radiusShockWave, float radiusBurnWave, float force)
    {
        _barrelPool = barrelPool;
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
            Debug.Log($" hitCollidersAfterSphereCast {_hitCollidersAfterSphereCast.Count}");
            for (int i = 0; i < _hitCollidersAfterSphereCast.Count; i++)
            {

                if (TryHitableInteract(_hitCollidersAfterSphereCast[i]) == false)
                {
                    TryInteractOther(_hitCollidersAfterSphereCast[i]); 
                }
            }
        }
    }
    private bool TryHitableInteract(Collider2D collider2D)
    {
        if (collider2D.transform.parent.TryGetComponent(out Barrel barrel))
        {
            Debug.Log($"      barrel");

            _forceShockWaveInRadius = CalculateForceShockWaveInRadius(barrel.Position);
            if (barrel.IsBroken == false)
            {
                AddForceBlastWaveToBarrel(barrel);
                barrel.TryBreakOnExplosion(_forceShockWaveInRadius);
            }
            return true;

        }
        else if (collider2D.transform.parent.TryGetComponent(out IHitable hitable))
        {
            Debug.Log($"    hitable");

            if (hitable.IsBroken == false)
            {
                _forceShockWaveInRadius = CalculateForceShockWaveInRadius(hitable.Position);
                if (hitable.TryBreakOnImpact(_forceShockWaveInRadius))
                {
                    AddForceBlastWaveToDebris(hitable.DebrisFragments);
                }
                else
                {
                    AddForceBlastWaveToWholeObject(hitable);
                }
            }

            return true;
        }
        else
        {
            Debug.Log($"    HitableInteract   false");

            return false;
        }
    }

    private void TryInteractOther(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            var force = CalculateDirectionBlastWave(rigidbody2D.position, _transformPointReference.position)
                    * CalculateForceShockWaveInRadius(rigidbody2D.position);
            rigidbody2D.AddForce(force);
            
            TryAddEffectToDebrisPiece(rigidbody2D.transform);
            
            Debug.Log($"    force  {force}");

        }
    }
    private void AddForceBlastWaveToDebris(IReadOnlyList<DebrisFragment> debrisFragments)
    {
        for (int i = 0; i < debrisFragments.Count; i++)
        {
            debrisFragments[i].TryAddForce(
                CalculateDirectionBlastWave(debrisFragments[i].FragmentTransform.position, _transformPointReference.position)
                * CalculateForceShockWaveInRadius(debrisFragments[i].FragmentTransform.position));
            TryAddEffectToDebrisPiece(debrisFragments[i].FragmentTransform);



            var a = CalculateDirectionBlastWave(debrisFragments[i].FragmentTransform.position,
                        _transformPointReference.position)
                    * CalculateForceShockWaveInRadius(debrisFragments[i].FragmentTransform.position);
            Debug.Log($"debrisFragment   index: {i}      force:{a}");
        }
    }
    
    private void TryAddEffectToDebrisPiece(Transform transform)
    {
        float distance = CalculateDistance(transform.position);
        if (transform.childCount < 1 && distance < _radiusBurnWave)
        {
            _barrelPool.GetDebrisBarrelEffect(transform).PlayEffect(CalculateIntensitySmoke(distance));
        }
    }
    private void AddForceBlastWaveToWholeObject(IHitable hitable)
    {
        hitable.AddForce(
            CalculateDirectionBlastWave(hitable.Position, _transformPointReference.position)
            * _forceShockWaveInRadius);
    }
    private void AddForceBlastWaveToBarrel(Barrel barrel)
    {
        barrel.AddForce(
            CalculateDirectionBlastWave(barrel.Position, _transformPointReference.position)
            * _forceShockWaveInRadius);
    }
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