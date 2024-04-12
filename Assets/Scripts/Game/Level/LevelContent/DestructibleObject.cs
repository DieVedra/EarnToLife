using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DestructibleObject : MonoBehaviour
{
    protected readonly float ForceMultiplierWholeObject = 3f;
    private readonly float _delayChangeLayer = 1f;
    [SerializeField] private int _layerDebris;
    [SerializeField] private Transform _debrisParent;
    [SerializeField] private Transform _wholeObjectTransform;
    [SerializeField] protected float Hardness;
    [Inject(Id = "DebrisParent")] private Transform _debrisParentForDestroy;
    protected List<DebrisFragment> FragmentsDebris;
    protected bool ObjectIsBroken;
    protected event Action OnDebrisHit;
    protected void Destruct()
    {
        _wholeObjectTransform.gameObject.SetActive(false);
        _debrisParent.gameObject.SetActive(true);
        AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound();
        ObjectIsBroken = true;
    }
    private void AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound()
    {
        AddRigidBodiesToDebrisAndSetParents();
        SubscribeDebrisForHitSound();
    }
    private void AddRigidBodiesToDebrisAndSetParents()
    {
        for (int i = 0; i < FragmentsDebris.Count; i++)
        {
            AddRigidBodyTo(FragmentsDebris[i]);
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
            FragmentsDebris.Add(new DebrisFragment(debris.GetChild(i)));
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
        for (int i = 0; i < FragmentsDebris.Count; i++)
        {
            FragmentsDebris[i].SubscribeFragment(DebrisHit, _layerDebris, _delayChangeLayer);
        }
    }
    private void DebrisHit()
    {
        OnDebrisHit?.Invoke();
    }
    protected void OnEnable()
    {
        ObjectIsBroken = false;
        FragmentsDebris = new List<DebrisFragment>();
        CollectDebrisChilds(_debrisParent);
    }
    protected void OnDisable()
    {
        for (int i = 0; i < FragmentsDebris.Count; i++)
        {
            FragmentsDebris[i].Dispose();
        }
    }
}