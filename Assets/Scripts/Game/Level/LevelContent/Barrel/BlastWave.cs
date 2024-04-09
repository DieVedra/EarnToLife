using System.Collections.Generic;
using UnityEngine;

public class BlastWave
{
    private readonly float _pointReference = 0f;
    private readonly float _force;
    private readonly float _radiusShockWave;
    private readonly Transform _transformPointReference;
    private readonly AnimationCurve _extinctionBlastWaveCurve;
    private float _forceShockWaveInRadius;
    private Collider2D[] _hitCollidersAfterSphereCast;

    public BlastWave(Transform transformPointReference, AnimationCurve extinctionBlastWaveCurve,
        float radiusShockWave, float force)
    {
        _transformPointReference = transformPointReference;
        _extinctionBlastWaveCurve = extinctionBlastWaveCurve;
        _radiusShockWave = radiusShockWave;
        _force = force;
    }

    public void InteractWithBlastWave(IReadOnlyList<DebrisFragment> debrisFragmentsThisObject)
    {
        if (TryCastSphere())
        {
            for (int i = 0; i < _hitCollidersAfterSphereCast.Length; i++)
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
        if (collider2D.transform.parent.TryGetComponent(out IHitable hitable))
        {
            if (hitable.IsBroken == false)
            {
                _forceShockWaveInRadius = CalculateForceShockWaveInRadius(hitable.Position);
                if (hitable.TryBreakOnImpact(_forceShockWaveInRadius))
                {
                    AddForceBlastWaveToDebris(hitable.DebrisFragments);
                }
                else
                {
                    var a = CalculateDirectionBlastWave(hitable.Position, _transformPointReference.position) *
                            _forceShockWaveInRadius;
                    AddForceBlastWaveToWholeObject(hitable, a);
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private void TryInteractOther(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            var a = CalculateDirectionBlastWave(rigidbody2D.position, _transformPointReference.position)
                    * CalculateForceShockWaveInRadius(rigidbody2D.position);
            rigidbody2D.AddForce(a);
        }
    }
    private void AddForceBlastWaveToDebris(IReadOnlyList<DebrisFragment> debrisFragments)
    {
        for (int i = 0; i < debrisFragments.Count; i++)
        {
            debrisFragments[i].TryAddForce(
                CalculateDirectionBlastWave(debrisFragments[i].FragmentTransform.position, _transformPointReference.position)
                * CalculateForceShockWaveInRadius(debrisFragments[i].FragmentTransform.position));
        }
    }

    private void AddForceBlastWaveToWholeObject(IHitable hitable, Vector2 force)
    {
        hitable.AddForce(
            CalculateDirectionBlastWave(hitable.Position, _transformPointReference.position)
            * CalculateForceShockWaveInRadius(hitable.Position));
    }
    private bool TryCastSphere()
    {
        _hitCollidersAfterSphereCast = Physics2D.OverlapCircleAll(_transformPointReference.position, _radiusShockWave);
        if (_hitCollidersAfterSphereCast.Length > 0)
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
        float distance = Vector2.Distance(_transformPointReference.position, positionHit);
        float valueForCurve = Mathf.InverseLerp(_radiusShockWave, _pointReference, distance);
        float forceMultiplier = _extinctionBlastWaveCurve.Evaluate(valueForCurve);
        float forceShockWave = _force * forceMultiplier;
        return forceShockWave;
    }

    private Vector2 CalculateDirectionBlastWave(Vector2 hitPoint, Vector2 referencePoint)
    {
        return hitPoint - referencePoint;
    }
}