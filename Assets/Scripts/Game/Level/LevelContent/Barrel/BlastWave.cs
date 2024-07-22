using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
    private List<DebrisFragment> _debrisFragments;
    private CancellationTokenSource _cancellationTokenSource;
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

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public void InteractWithBlastWave()
    {
        if (TryCastSphere())
        {
            _debrisFragments = new List<DebrisFragment>(_hitCollidersAfterSphereCast.Count);
            _cancellationTokenSource = new CancellationTokenSource();
            TryInteract().Forget();
            // TryInteract();
        }
    }

    private async UniTaskVoid TryInteract()
    {
        TryInteractPart1();
        await UniTask.NextFrame(_cancellationTokenSource.Token);
        TryInteractPart2AddForceBlastWaveToDebris();
    }
    private void TryInteractPart1()
    {
        for (int i = 0; i < _hitCollidersAfterSphereCast.Count; i++)
        {
            if (_hitCollidersAfterSphereCast[i].TryGetComponent(out DebrisFragment debrisFragment))
            {
                // var forceDirection = CalculateDirectionBlastWave(debrisFragment.FragmentTransform.position, _transformPointReference.position)
                //                      * CalculateForceShockWaveInRadius(debrisFragment.FragmentTransform.position);
                // debrisFragment.Rigidbody2D.AddForce(forceDirection);
                // TryAddEffectToDebrisPiece(debrisFragment);
                
                _debrisFragments.Add(debrisFragment);
                
            }
            else if (_hitCollidersAfterSphereCast[i].transform.parent.TryGetComponent(out IExplosive explosive))
            {
                if (explosive.TryBreakOnExplosion(
                    CalculateDirectionBlastWave(explosive.Position, _transformPointReference.position),
                    CalculateForceShockWaveInRadius(explosive.Position)) == true)
                {
                    // AddForceBlastWaveToDebris(explosive.DebrisFragments);
                    _debrisFragments.AddRange(explosive.DebrisFragments);

                }
            }
        }
    }

    private void TryInteractPart2AddForceBlastWaveToDebris()
    {
        for (int i = 0; i < _debrisFragments.Count; i++)
        {
            _debrisFragments[i].TryAddForce(
                CalculateDirectionBlastWave(_debrisFragments[i].FragmentTransform.position, _transformPointReference.position)
                * CalculateForceShockWaveInRadius(_debrisFragments[i].FragmentTransform.position));
            TryAddEffectToDebrisPiece(_debrisFragments[i]);
        }
    }
    // private async UniTaskVoid TryInteract()
    // {
    //     for (int i = 0; i < _hitCollidersAfterSphereCast.Count; i++)
    //     {
    //         if (i % 3 == 0)
    //         {
    //             await UniTask.NextFrame(_cancellationTokenSource.Token);
    //         }
    //
    //         if (_hitCollidersAfterSphereCast[i].TryGetComponent(out DebrisFragment debrisFragment))
    //         {
    //             var forceDirection = CalculateDirectionBlastWave(debrisFragment.FragmentTransform.position, _transformPointReference.position)
    //                                  * CalculateForceShockWaveInRadius(debrisFragment.FragmentTransform.position);
    //             debrisFragment.Rigidbody2D.AddForce(forceDirection);
    //             TryAddEffectToDebrisPiece(debrisFragment);
    //         }
    //         else if (_hitCollidersAfterSphereCast[i].transform.parent.TryGetComponent(out IExplosive explosive))
    //         {
    //             if (explosive.TryBreakOnExplosion(
    //                 CalculateDirectionBlastWave(explosive.Position, _transformPointReference.position),
    //                 CalculateForceShockWaveInRadius(explosive.Position)) == true)
    //             {
    //                 AddForceBlastWaveToDebris(explosive.DebrisFragments);
    //             }
    //         }
    //     }
    // }
    // private void AddForceBlastWaveToDebris()
    // {
    //     for (int i = 0; i < _debrisFragments.Count; i++)
    //     {
    //         _debrisFragments[i].TryAddForce(
    //             CalculateDirectionBlastWave(_debrisFragments[i].FragmentTransform.position, _transformPointReference.position)
    //             * CalculateForceShockWaveInRadius(_debrisFragments[i].FragmentTransform.position));
    //         TryAddEffectToDebrisPiece(_debrisFragments[i]);
    //     }
    // }
    
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