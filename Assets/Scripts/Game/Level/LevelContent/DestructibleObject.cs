using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] protected Transform WholeObjectTransform;
    [SerializeField] protected float Hardness = 30f;
    [SerializeField] private Transform _debrisParentLocal;
    [SerializeField] private bool _isGrouped = false;
    private int _layerDebris;
    private int _layerCar;
    private DestructionsSignal _destructionsSignal;
    private CompositeDisposable _compositeDisposable;
    
    private Action<float> _debrisHitSound;

    protected readonly float ForceMultiplierWholeObject = 3f;
    protected Rigidbody2D Rigidbody2D;
    protected Transform TransformBase;
    protected bool ObjectIsBroken;
    
    protected List<DebrisFragment> FragmentsDebris;
    public bool IsGrouped => _isGrouped;

    [Inject]
    private void Construct(LayersProvider layersProvider, DestructionsSignal destructionsSignal, ILevel level)
    {
        _layerDebris = layersProvider.LayerDebris;
        _layerCar = layersProvider.CarLayer;
        _destructionsSignal = destructionsSignal;
    }

    protected void Init(Action<float> debrisHitSound)
    {
        TransformBase = transform;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        FragmentsDebris = new List<DebrisFragment>();
        _debrisHitSound = debrisHitSound;
        CollectDebrisChildsAndInitTheir(_debrisParentLocal);
    }
    protected void Destruct()
    {
        WholeObjectTransform.gameObject.SetActive(false);
        Rigidbody2D.simulated = false;
        Rigidbody2D.isKinematic = true;
        ObjectIsBroken = true;
        
        _debrisParentLocal.gameObject.SetActive(true);
        foreach (DebrisFragment fragmentDebris in FragmentsDebris)
        {
            fragmentDebris.Activate();
        }
        _destructionsSignal.OnDestruction?.Invoke();
    }
    private void CollectDebrisChildsAndInitTheir(Transform debris)
    {
        for (int i = 0; i < debris.childCount; i++)
        {
            if (debris.GetChild(i).childCount > 0)
            {
                CollectDebrisChildsAndInitTheir(debris.GetChild(i));
            }
            else
            {
                TryInitDebrisChild(debris.GetChild(i));
            }
        }
    }

    private void TryInitDebrisChild(Transform child)
    {
        if (child.TryGetComponent(out Collider2D collider2D) == true)
        {
            DebrisFragment debrisFragment = child.AddComponent<DebrisFragment>();
            debrisFragment.Init(_debrisHitSound, _layerDebris, _layerCar);
            FragmentsDebris.Add(debrisFragment);
        }
    }
    protected void OnEnable()
    {
        ObjectIsBroken = false;
    }
    protected void OnDestroy()
    {
        if (FragmentsDebris != null)
        {
            foreach (DebrisFragment fragmentDebris in FragmentsDebris)
            {
                fragmentDebris.Dispose();
            }
        }
        _compositeDisposable?.Clear();
    }
}