using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Level : MonoBehaviour, ILevel
{
    [SerializeField] private Transform _debrisParent;
    [SerializeField] private Transform _barrelPoolEffectsParent;
    private LevelPool _levelPool;
    public Transform DebrisParent => _debrisParent;
    public LevelPool LevelPool => _levelPool;
    public BarrelPool BarrelPool => _levelPool.BarrelPool;
    
    [Inject]
    public void Init(Factory factory, LevelPrefabsProvider levelPrefabsProvider)
    {
        _levelPool = new LevelPool(
            new BarrelPool(
                levelPrefabsProvider.LevelParticlesProvider.BarrelExplosion,
                levelPrefabsProvider.LevelParticlesProvider.DebrisBarrelEffect, factory, _barrelPoolEffectsParent)
            );
        
        
    }

    private void OnDisable()
    {
        BarrelPool.Dispose();
    }
}
