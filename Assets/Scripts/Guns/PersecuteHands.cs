using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersecuteHands
{
    private List<FollowPoints> _followPoints;
    public PersecuteHands(Transform shoulderSupport, Transform leftHand, Transform rightHand, Transform handlePoint, Transform forearmPoint, Transform buttPoint)
    {
        _followPoints = new List<FollowPoints>();

        FillInListPoints(buttPoint, shoulderSupport);
        FillInListPoints(leftHand, handlePoint);
        FillInListPoints(rightHand, forearmPoint);
    }
    public void Update()
    {
        for (int i = 0; i < _followPoints.Count; i++)
        {
            _followPoints[i]._toFollow.position = _followPoints[i]._target.position;
        }
    }
    private void FillInListPoints(Transform toFollow, Transform target)
    {
        _followPoints.Add(new FollowPoints(toFollow, target));
    }
    private class FollowPoints
    {
        public Transform _toFollow;
        public Transform _target;
        public FollowPoints(Transform toFollow, Transform target)
        {
            _toFollow = toFollow;
            _target = target;
        }
    }
}
