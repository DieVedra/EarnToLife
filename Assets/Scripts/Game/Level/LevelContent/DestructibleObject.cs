using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class DestructibleObject : MonoBehaviour
{
    protected readonly float ForceMultiplierWholeObject = 3f;
    private readonly float _addXRange = 30f;
    private int _layerDebris;
    private int _layerCar;
    [SerializeField] protected Transform WholeObjectTransform;
    [SerializeField] protected float Hardness = 30f;
    [SerializeField] private Transform _debrisParentLocal;
    protected Rigidbody2D Rigidbody2D;
    protected Transform TransformBase;
    protected List<DebrisFragment> FragmentsDebris;
    protected Action DebrisHitSound;
    protected bool ObjectIsBroken;
    private Transform _cameraTransform;
    private DestructionsSignal _destructionsSignal;
    private CompositeDisposable _compositeDisposable;
    
    [Inject]
    private void Construct(LayersProvider layersProvider, DestructionsSignal destructionsSignal, ILevel level)
    {
        _layerDebris = layersProvider.LayerDebris;
        _layerCar = layersProvider.CarLayer;
        _destructionsSignal = destructionsSignal;
        _cameraTransform = level.CameraTransform; 
        Rigidbody2D = GetComponent<Rigidbody2D>();
        FragmentsDebris = new List<DebrisFragment>();
        TransformBase = transform;
        CollectDebrisChilds(_debrisParentLocal);
        _compositeDisposable = new CompositeDisposable();
    }

    private void Start()
    {
        SubscribeEnableAndDisableObserve();
    }

    protected void Destruct()
    {
        WholeObjectTransform.gameObject.SetActive(false);
        _debrisParentLocal.gameObject.SetActive(true);
        AddRigidBodiesToDebrisAndSubscribeDebrisForHitSound();
        _destructionsSignal.OnDestruction?.Invoke();
        Rigidbody2D.isKinematic = true;
        Rigidbody2D.simulated = false;
        ObjectIsBroken = true;
    }
    private void SubscribeEnableAndDisableObserve()
    {
        gameObject.SetActive(false);
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (TransformBase.position.x  + _addXRange < _cameraTransform.position.x)
            {
                gameObject.SetActive(false);
            }
            else if(TransformBase.position.x < _cameraTransform.position.x + _addXRange)
            {
                gameObject.SetActive(true);
            }
        }).AddTo(_compositeDisposable);
    }
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
                    debrisFragment.Init(DebrisHitSound, _layerDebris, _layerCar);
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
        for (int i = 0; i < FragmentsDebris.Count; i++)
        {
            FragmentsDebris[i].Dispose();
        }
        _compositeDisposable.Clear();
    }
}