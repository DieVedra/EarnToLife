using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DestructibleObject : MonoBehaviour
{
    private readonly float _delayChangeLayer = 1f;
    [SerializeField] private int _layerDebris;
    [SerializeField] private Transform _debrisParent;
    [SerializeField] private Transform _wholeObjectTransform;
    [SerializeField] private float _hardness;
    [Inject(Id = "DebrisParent")] private Transform _debrisParentForDestroy;
    protected List<DebrisFragment> DebrisFragments;
    protected bool ObjectIsBroken;
    
    protected event Action OnDestruct;
    protected event Action OnDestructFail;
    protected event Action OnDebrisHit;
    protected bool TryDestruct(float normalImpulse)
    {
        if (normalImpulse > _hardness)
        {
            _wholeObjectTransform.gameObject.SetActive(false);
            _debrisParent.gameObject.SetActive(true);
            AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound();
            ObjectIsBroken = true;
            OnDestruct?.Invoke();
            return true;
        }
        else
        {
            OnDestructFail?.Invoke();
            return false;
        }
    }
    private void AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound()
    {
        AddRigidBodiesToDebrisAndSetParents();
        SubscribeDebrisForHitSound();
    }
    private void AddRigidBodiesToDebrisAndSetParents()
    {
        for (int i = 0; i < DebrisFragments.Count; i++)
        {
            AddRigidBodyTo(DebrisFragments[i]);
            // SetDebrisParent(_debris[i].FragmentTransform);
        }
    }
    private void CollectDebrisChilds(Transform debris)
    {
        for (int i = 0; i < debris.childCount; i++)
        {
            if (debris.GetChild(i).childCount > 0)
            {
                CollectDebrisChilds(debris.GetChild(i));
            }
            DebrisFragments.Add(new DebrisFragment(debris.GetChild(i)));
        }
    }
    private void AddRigidBodyTo(DebrisFragment fragment)
    {
        fragment.InitRigidBody();
    }
    private void SetDebrisParent(Transform chip)
    {
        chip.SetParent(_debrisParentForDestroy);
    }
    private void SubscribeDebrisForHitSound()
    {
        for (int i = 0; i < DebrisFragments.Count; i++)
        {
            DebrisFragments[i].SubscribeFragment(DebrisHit, _layerDebris, _delayChangeLayer);
        }
    }
    private void DebrisHit()
    {
        OnDebrisHit?.Invoke();
    }
    protected void OnEnable()
    {
        ObjectIsBroken = false;
        DebrisFragments = new List<DebrisFragment>();
        CollectDebrisChilds(_debrisParent);
    }
    protected void OnDisable()
    {
        for (int i = 0; i < DebrisFragments.Count; i++)
        {
            DebrisFragments[i].Dispose();
        }
    }
}