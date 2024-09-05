using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Level : MonoBehaviour, ILevel
{
    [SerializeField, HorizontalLine(color: EColor.White)] private DebrisParent _debrisParent;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _debrisPoolEffectsParent;
    [SerializeField] private Transform _barrelPoolEffectsParent;
    [SerializeField] private Transform _bloodEffectsParent;
    [SerializeField, HorizontalLine(color: EColor.Yellow)] private LevelAudio _levelAudio;
    [SerializeField, HorizontalLine(color: EColor.Orange)] private LevelBlock[] _levelBlocks;
    private LevelBlocksHandler _levelBlocksHandler;
    private LevelPool _levelPool;
    public DebrisParent DebrisParent => _debrisParent;
    public LevelPool LevelPool => _levelPool;
    public LevelAudio LevelAudio =>_levelAudio;
    public Transform CameraTransform => _cameraTransform;
    
    [Inject]
    public void Construct(LevelPrefabsProvider levelPrefabsProvider, AudioClipProvider audioClipProvider, TimeScaleSignal timeScaleSignal, IGlobalAudio globalAudio)
    {
        _levelAudio.Init(audioClipProvider, timeScaleSignal, globalAudio);
        _levelPool = new LevelPool(
            new BarrelPool(
                levelPrefabsProvider.LevelParticlesProvider.BarrelExplosion,
                levelPrefabsProvider.LevelParticlesProvider.BurnEffect, _barrelPoolEffectsParent),
            new ZombiePool(levelPrefabsProvider.LevelParticlesProvider.BloodHitEffect,
                levelPrefabsProvider.LevelParticlesProvider.BloodEffect, _bloodEffectsParent),
            new DebrisPool(
                levelPrefabsProvider.LevelParticlesProvider.BurnEffect,
                levelPrefabsProvider.LevelParticlesProvider.SmokeEffect,
                levelPrefabsProvider.LevelParticlesProvider.DebrisEffect, _debrisPoolEffectsParent)
            );
        _levelBlocksHandler = new LevelBlocksHandler(_levelBlocks, _cameraTransform);
    }
    private void OnDisable()
    {
        LevelPool?.Dispose();
        _levelBlocksHandler?.Dispose();
        _levelAudio?.Dispose();
    }
}
