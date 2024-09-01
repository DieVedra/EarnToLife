using System.Collections.Generic;
using UniRx;
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
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private List<DebrisFragment> _debrisFragments;
    private List<Vector2> _debrisForces;
    private float _forceShockWaveInRadius;
    private int _calculatePart = 0;

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

    public void Dispose()
    {
        _compositeDisposable.Clear();
    }
    public void InteractWithBlastWave()
    {
        if (TryCastSphere())
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                switch (_calculatePart)
                {
                    case 0:
                        _debrisFragments = new List<DebrisFragment>(_hitCollidersAfterSphereCast.Count);
                        _debrisForces = new List<Vector2>(_hitCollidersAfterSphereCast.Count);
                        Sorting();
                        _calculatePart++;
                        break;
                    case 1:
                        CalculateForces();
                        _calculatePart++;
                        break;
                    case 2:
                        AddForces();
                        _compositeDisposable.Clear();
                        break;
                }
            }).AddTo(_compositeDisposable);
        }
    }

    private void Sorting()
    {
        for (int i = 0; i < _hitCollidersAfterSphereCast.Count; i++)
        {
            if (_hitCollidersAfterSphereCast[i].TryGetComponent(out DebrisFragment debrisFragment))
            {
                _debrisFragments.Add(debrisFragment);

            }
            else if (_hitCollidersAfterSphereCast[i].transform.parent.TryGetComponent(out IExplosive explosive))
            {
                if (explosive.TryBreakOnExplosion(
                    CalculateDirectionBlastWave(explosive.Position, _transformPointReference.position),
                    CalculateForceShockWaveInRadius(explosive.Position)) == true)
                {
                    _debrisFragments.AddRange(explosive.DebrisFragments);
                }
            }
        }
    }

    private void AddForces()
    {
        for (int i = 0; i < _debrisFragments.Count; i++)
        {
            _debrisFragments[i].TryAddForce(_debrisForces[i]);
            TryAddEffectToDebrisPiece(_debrisFragments[i]);
        }
    }
    private void CalculateForces()
    {
        for (int i = 0; i < _debrisFragments.Count; i++)
        {
            _debrisForces.Add(CalculateDirectionBlastWave(_debrisFragments[i].FragmentTransform.position, _transformPointReference.position)
                              * CalculateForceShockWaveInRadius(_debrisFragments[i].FragmentTransform.position));
        }
    }
    
    private void TryAddEffectToDebrisPiece(DebrisFragment debrisFragment)
    {
        float distance = CalculateDistance(debrisFragment.FragmentTransform.position);
        if (distance <= _radiusBurnWave)
        {
            _debrisPoolEffects.GetDebrisBarrelEffect().PlayEffectTo(debrisFragment, CalculateIntensity(distance), true);
        }
        else
        {
            _debrisPoolEffects.GetDebrisBarrelEffect().PlayEffectTo(debrisFragment, CalculateIntensity(distance), false);
        }
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
    private float CalculateIntensity(float distance)
    {
        return Mathf.InverseLerp(_radiusShockWave, _pointReference, distance);
    }
    private Vector2 CalculateDirectionBlastWave(Vector2 hitPoint, Vector2 referencePoint)
    {
        return hitPoint - referencePoint;
    }
}