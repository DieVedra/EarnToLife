using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarGunDetector
{
    private readonly float _radiusDetection;
    private readonly float _deadZoneDetectionValue;
    private readonly Transform _visionPointTransform;
    private readonly ContactFilter2D _contactFilter2D;
    private List<Collider2D> _colliders = new List<Collider2D>();
    private List<CarGunTarget> _targets;
    public int GetCurrentCountTargets => _targets?.Count ?? 0;
    public int GetMaxIndexTargets => _targets.Count - 1;

    public CarGunDetector(Transform viewPoint, LayerMask layerMask, float distanceDetection, float deadZoneDetectionValue)
    {
        _visionPointTransform = viewPoint;
        _radiusDetection = distanceDetection;
        _deadZoneDetectionValue = deadZoneDetectionValue;
        _contactFilter2D = new ContactFilter2D();
        _contactFilter2D.SetLayerMask(layerMask);
    }
    public bool TryFindTarget()
    {
        if (CreateTargets())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public CarGunTarget GetTarget(int currentTargetIndex)
    {
        if (GetMaxIndexTargets < currentTargetIndex)
        {
            return _targets[GetMaxIndexTargets];
        }
        else
        {
            return _targets[currentTargetIndex];
        }
    }
    private void AddTarget(CarGunTarget carGunTarget)
    {
        if (CheckRelevanceTarget(carGunTarget.Target))
        {
            _targets.Add(carGunTarget);
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
    private bool CreateTargets()
    {
        if (Physics2D.OverlapCircle(_visionPointTransform.position, _radiusDetection, _contactFilter2D, _colliders) > 0)
        {
            _targets = new List<CarGunTarget>();
            for (int i = 0; i < _colliders.Count; i++)
            {
                if (_colliders[i].transform.parent.gameObject.TryGetComponent(out IShotable shotable))
                {
                    AddTarget(new CarGunTarget(shotable, _colliders[i]));
                }
            }
            if (_targets.Count > 0)
            {
                _targets = SortByDistance(_targets);
                return true;
            }
            else
            {
                _targets = null;
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private List<CarGunTarget> SortByDistance(List<CarGunTarget> targets)
    {
        return targets.OrderBy(obj => Vector2.Distance(obj.TargetCollider.transform.position, _visionPointTransform.position)).ToList();
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
