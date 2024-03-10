using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGunDetector
{
    private readonly float _radiusDetection;
    private readonly float _deadZoneDetectionValue;
    private Transform _visionPointTransform;
    private LayerMask _layerMask;
    private Collider2D[] _hitCollidersAfterSphereCast;
    private List<CarGunTarget> _targetsAfterSorting;
    public int GetCurrentCountTargetsAfterSorted => _targetsAfterSorting?.Count ?? 0;

    public CarGunDetector(Transform viewPoint, LayerMask layerMask, float distanceDetection, float deadZoneDetectionValue)
    {
        _visionPointTransform = viewPoint;
        _layerMask = layerMask;
        _radiusDetection = distanceDetection;
        _deadZoneDetectionValue = deadZoneDetectionValue;
    }
    public bool TryFindTarget()
    {
        if (TryCastSphere())
        {
            if (SortingTargets())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public CarGunTarget GetSortedTarget(int currentTargetIndex)
    {
        if (_targetsAfterSorting.Count - 1 < currentTargetIndex)
        {
            return _targetsAfterSorting[_targetsAfterSorting.Count - 1];
        }
        else
        {
            return _targetsAfterSorting[currentTargetIndex];
        }
    }
    private bool SortingTargets()
    {
        _targetsAfterSorting = new List<CarGunTarget>(_hitCollidersAfterSphereCast.Length);
        for (int i = 0; i < _hitCollidersAfterSphereCast.Length; i++)
        {
            if (_hitCollidersAfterSphereCast[i].gameObject.TryGetComponent(out IShotable shotable))
            {
                AddTarget(new CarGunTarget(shotable, _hitCollidersAfterSphereCast[i]));
            }
        }
        if (_targetsAfterSorting.Count > 0)
        {
            return true;
        }
        else
        {
            _targetsAfterSorting = null;
            return false;
        }
    }
    private void AddTarget(CarGunTarget carGunTarget)
    {
        if (CheckRelevanceTarget(carGunTarget.Target))
        {
            _targetsAfterSorting.Add(carGunTarget);
        }
    }
    public bool CheckRelevanceTarget(IShotable target)
    {
        if (CheckDistance(target.TargetTransform.position.x) == true && CheckLiveTarget(target) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool TryCastSphere()
    {
        _hitCollidersAfterSphereCast = Physics2D.OverlapCircleAll(_visionPointTransform.position, _radiusDetection, _layerMask.value);
        if (_hitCollidersAfterSphereCast.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckDistance(float targetCoordX)
    {
        if (_visionPointTransform.position.x + _deadZoneDetectionValue < targetCoordX 
            & _visionPointTransform.position.x + _radiusDetection > targetCoordX)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckLiveTarget(IShotable shotable)
    {
        if (shotable.IsLive == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
