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
    [SerializeField] private LevelAudio _levelAudio;
    private LevelPool _levelPool;
    public Transform DebrisParent => _debrisParent;
    public LevelPool LevelPool => _levelPool;
    public LevelAudio LevelAudio =>_levelAudio;
    public BarrelPool BarrelPool => _levelPool.BarrelPool;
    
    [Inject]
    public void Init(Factory factory, LevelPrefabsProvider levelPrefabsProvider, AudioClipProvider audioClipProvider, IGlobalAudio globalAudio)
    {
        _levelPool = new LevelPool(
            new BarrelPool(
                levelPrefabsProvider.LevelParticlesProvider.BarrelExplosion,
                levelPrefabsProvider.LevelParticlesProvider.DebrisBarrelEffect, factory, _barrelPoolEffectsParent)
            );
        _levelAudio.Init(audioClipProvider, globalAudio);
    }

    private void OnDisable()
    {
        BarrelPool.Dispose();
    }
}
