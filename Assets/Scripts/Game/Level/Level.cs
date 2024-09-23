using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Level : MonoBehaviour, ILevel
{
    [SerializeField, HorizontalLine(color: EColor.White)] private DebrisKeeper _debrisKeeper;
    [SerializeField] private ActivityLevelContentHandler _activityLevelContentHandler;
    [SerializeField] private Transform _cameraTransform;
    
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _debrisPoolEffectsParent;
    [SerializeField] private Transform _barrelPoolEffectsParent;
    [SerializeField] private Transform _bloodEffectsParent;
    
    [SerializeField, HorizontalLine(color: EColor.Yellow)] private LevelAudio _levelAudio;
    
    [SerializeField, HorizontalLine(color: EColor.Orange)] private Transform _startLevelPoint;
    [SerializeField] private Transform _endLevelPoint;
    [SerializeField] private LevelBlock[] _levelBlocks;
    
    private LevelPool _levelPool;
    private LevelPrefabsProvider _levelPrefabsProvider;
    private AudioClipProvider _audioClipProvider;
    private TimeScaleSignal _timeScaleSignal;
    private IGlobalAudio _globalAudio;
    
    public DebrisKeeper DebrisKeeper => _debrisKeeper;
    public LevelPool LevelPool => _levelPool;
    public LevelAudio LevelAudio =>_levelAudio;
    public Transform CameraTransform => _cameraTransform;
    public Transform StartLevelPoint => _startLevelPoint;
    public Transform EndLevelPoint => _endLevelPoint;
    
    [Inject]
    public void Construct(LevelPrefabsProvider levelPrefabsProvider, AudioClipProvider audioClipProvider, TimeScaleSignal timeScaleSignal, IGlobalAudio globalAudio)
    {
        _levelPrefabsProvider = levelPrefabsProvider;
        _audioClipProvider = audioClipProvider;
        _timeScaleSignal = timeScaleSignal;
        _globalAudio = globalAudio;
        
        _levelPool = new LevelPool(
            new BarrelPool(
                _levelPrefabsProvider.LevelParticlesProvider.BarrelExplosion,
                _levelPrefabsProvider.LevelParticlesProvider.BurnEffect,
                _barrelPoolEffectsParent),
            new ZombiePool(_levelPrefabsProvider.LevelParticlesProvider.BloodHitEffect,
                _levelPrefabsProvider.LevelParticlesProvider.BloodEffect,
                _bloodEffectsParent),
            new DebrisPool(
                _levelPrefabsProvider.LevelParticlesProvider.BurnEffect,
                _levelPrefabsProvider.LevelParticlesProvider.SmokeEffect,
                _levelPrefabsProvider.LevelParticlesProvider.DebrisEffect,
                _debrisPoolEffectsParent)
        );
        
        _levelAudio.Init(_audioClipProvider, _timeScaleSignal, _globalAudio);
        _activityLevelContentHandler.Init(_debrisKeeper);
    }

    // private void Awake()
    // {
    //     _levelAudio.Init(_audioClipProvider, _timeScaleSignal, _globalAudio);
    //     _activityLevelContentHandler.Init(_debrisKeeper);
    // }

    private void OnDisable()
    {
        LevelPool?.Dispose();
        _levelAudio?.Dispose();
    }
}
