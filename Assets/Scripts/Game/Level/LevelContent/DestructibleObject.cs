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

    // private readonly float _addXRange = 30f;
    private int _layerDebris;
    private int _layerCar;
    // private Transform _cameraTransform;
    private DestructionsSignal _destructionsSignal;
    private CompositeDisposable _compositeDisposable;
    private Action<float> _debrisHitSound;

    protected readonly float ForceMultiplierWholeObject = 3f;
    protected Rigidbody2D Rigidbody2D;
    protected Transform TransformBase;
    protected bool ObjectIsBroken;
    
    protected List<DebrisFragment> FragmentsDebris;

    [Inject]
    private void Construct(LayersProvider layersProvider, DestructionsSignal destructionsSignal, ILevel level)
    {
        _layerDebris = layersProvider.LayerDebris;
        _layerCar = layersProvider.CarLayer;
        _destructionsSignal = destructionsSignal;
        // _cameraTransform = level.CameraTransform;
    }

    protected void Init(Action<float> debrisHitSound)
    {
        TransformBase = transform;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        FragmentsDebris = new List<DebrisFragment>();
        CollectDebrisChilds(_debrisParentLocal);
        _debrisHitSound = debrisHitSound;
        InitDebris();
    }

    // private void Start()
    // {
    //     SubscribeEnableAndDisableObserve();
    //     
    // }

    private void InitDebris()
    {
        foreach (var debrisFragment in FragmentsDebris)
        {
            debrisFragment.Init(_debrisHitSound, _layerDebris, _layerCar);
        }
    }
    protected void Destruct()
    {
        WholeObjectTransform.gameObject.SetActive(false);
        _debrisParentLocal.gameObject.SetActive(true);
        Rigidbody2D.simulated = false;
        Rigidbody2D.isKinematic = true;
        ObjectIsBroken = true;
        AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound();
        _destructionsSignal.OnDestruction?.Invoke();
    }
    // private void SubscribeEnableAndDisableObserve()
    // {
    //     TransformBase = transform;
    //     _compositeDisposable = new CompositeDisposable();
    //     gameObject.SetActive(false);
    //     Observable.EveryUpdate().Subscribe(_ =>
    //     {
    //         if (TransformBase.position.x  + _addXRange < _cameraTransform.position.x)
    //         {
    //             gameObject.SetActive(false);
    //         }
    //         else if(TransformBase.position.x - _addXRange < _cameraTransform.position.x)
    //             // else if(TransformBase.position.x < _cameraTransform.position.x + _addXRange)
    //         {
    //             gameObject.SetActive(true);
    //         }
    //     }).AddTo(_compositeDisposable);
    // }
    private void AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound()
    {
        AddRigidBodiesToDebris();
        SubscribeDebrisForHitSound();
    }
    private void AddRigidBodiesToDebris()
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
                    // debrisFragment.Init(_debrisHitSound, _layerDebris, _layerCar);
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
            FragmentsDebris[i].SubscribeFragment();
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
            for (int i = 0; i < FragmentsDebris.Count; i++)
            {
                FragmentsDebris[i].Dispose();
            }
        }

        _compositeDisposable?.Clear();
    }
}