using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    protected readonly float ForceMultiplierWholeObject = 3f;
    private readonly float _delayChangeLayer = 0.3f;
    private readonly float _addXRange = 20f;
    [SerializeField, Layer] private int _layerDebris;
    [SerializeField] protected Transform WholeObjectTransform;
    [SerializeField] protected float Hardness;
    [SerializeField] private Transform _debrisParentLocal;
    protected Rigidbody2D Rigidbody2D;
    protected Transform CameraTransform;
    protected List<DebrisFragment> FragmentsDebris;
    protected bool ObjectIsBroken;
    protected Action DebrisHitSound;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    protected Transform TransformBase;
    private void Awake()
    {
        FragmentsDebris = new List<DebrisFragment>();
        TransformBase = transform;
        CollectDebrisChilds(_debrisParentLocal);
    }

    protected void Destruct()
    {
        WholeObjectTransform.gameObject.SetActive(false);
        _debrisParentLocal.gameObject.SetActive(true);
        AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound();
        Rigidbody2D.isKinematic = true;
        Rigidbody2D.simulated = false;
        ObjectIsBroken = true;
        Observable.EveryUpdate().Do(_ =>
        {
            if (TransformBase.position.x + _addXRange < CameraTransform.position.x)
            {
                _compositeDisposable.Clear();
                gameObject.SetActive(false);
            }
        }).Subscribe().AddTo(_compositeDisposable);
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
                if (debris.GetChild(i).TryGetComponent(out Collider2D collider2D) == true)
                {
                    DebrisFragment debrisFragment = debris.GetChild(i).AddComponent<DebrisFragment>();
                    debrisFragment.Init();
                    FragmentsDebris.Add(debrisFragment);
                }
            }
        }
    }
    private void AddRigidBodyTo(DebrisFragment fragment)
    {
        fragment.InitRigidBody();
    }
    private void SubscribeDebrisForHitSound()
    {
        for (int i = 0; i < FragmentsDebris.Count; i++)
        {
            FragmentsDebris[i].SubscribeFragment(DebrisHitSound, _layerDebris, _delayChangeLayer);
        }
    }
    protected void OnEnable()
    {
        ObjectIsBroken = false;
    }
    protected void OnDisable()
    {
        for (int i = 0; i < FragmentsDebris.Count; i++)
        {
            FragmentsDebris[i].Dispose();
        }
        _compositeDisposable.Clear();
    }
}