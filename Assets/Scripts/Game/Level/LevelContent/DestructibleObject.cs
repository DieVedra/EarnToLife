using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    protected readonly float ForceMultiplierWholeObject = 3f;
    private readonly float _delayChangeLayer = 1f;
    [SerializeField, Layer] private int _layerDebris;
    [SerializeField] protected Transform WholeObjectTransform;
    [SerializeField] protected float Hardness;
    [SerializeField] private Transform _debrisParentLocal;
    protected Rigidbody2D Rigidbody2D;
    protected Transform DebrisParentForDestroy;
    protected List<DebrisFragment> FragmentsDebris;
    protected bool ObjectIsBroken;
    protected event Action OnDebrisHit;

    protected void Destruct()
    {
        WholeObjectTransform.gameObject.SetActive(false);
        _debrisParentLocal.gameObject.SetActive(true);
        AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound();
        Rigidbody2D.isKinematic = true;
        Rigidbody2D.simulated = false;
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
            else
            {
                FragmentsDebris.Add(new DebrisFragment(debris.GetChild(i)));
            }
        }
    }
    private void AddRigidBodyTo(DebrisFragment fragment)
    {
        fragment.InitRigidBody();
    }
    private void SetDebrisParent(Transform chip)
    {
        chip.SetParent(DebrisParentForDestroy);
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
        CollectDebrisChilds(_debrisParentLocal);
    }
    protected void OnDisable()
    {
        for (int i = 0; i < FragmentsDebris.Count; i++)
        {
            FragmentsDebris[i].Dispose();
        }
    }
}