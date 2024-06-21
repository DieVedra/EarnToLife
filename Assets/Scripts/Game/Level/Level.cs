using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Level : MonoBehaviour, ILevel
{
    [SerializeField, HorizontalLine(color: EColor.White)] private Transform _debrisParent;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _debrisPoolEffectsParent;
    [SerializeField] private Transform _barrelPoolEffectsParent;
    [SerializeField] private Transform _bloodEffectsParent;
    [SerializeField, HorizontalLine(color: EColor.Yellow)] private LevelAudio _levelAudio;
    [SerializeField, HorizontalLine(color: EColor.Orange)] private LevelBlock[] _levelBlocks;
    private LevelBlocksHandler _levelBlocksHandler;
    private LevelPool _levelPool;
    public Transform DebrisParent => _debrisParent;
    public LevelPool LevelPool => _levelPool;
    public LevelAudio LevelAudio =>_levelAudio;
    public Transform CameraTransform => _cameraTransform;
    
    [Inject]
    public void Construct(Factory factory, LevelPrefabsProvider levelPrefabsProvider, AudioClipProvider audioClipProvider, IGlobalAudio globalAudio)
    {
        _levelAudio.Init(audioClipProvider, globalAudio);
        _levelPool = new LevelPool(
            new BarrelPool(
                levelPrefabsProvider.LevelParticlesProvider.BarrelExplosion,
                levelPrefabsProvider.LevelParticlesProvider.BurnEffect,
                factory, _barrelPoolEffectsParent),
            new ZombiePool(levelPrefabsProvider.LevelParticlesProvider.BloodHitEffect,
                levelPrefabsProvider.LevelParticlesProvider.BloodEffect,
                factory, _bloodEffectsParent),
            new DebrisPool(
                levelPrefabsProvider.LevelParticlesProvider.BurnEffect,
                levelPrefabsProvider.LevelParticlesProvider.SmokeEffect,
                levelPrefabsProvider.LevelParticlesProvider.DebrisEffect,
                factory, _debrisPoolEffectsParent)
            );
        _levelBlocksHandler = new LevelBlocksHandler(_levelBlocks, _cameraTransform);
    }
    private void OnDisable()
    {
        LevelPool.Dispose();
        _levelBlocksHandler.Dispose();
    }
}
