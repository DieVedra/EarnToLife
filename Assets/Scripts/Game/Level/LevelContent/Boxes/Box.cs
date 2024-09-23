using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Box : DestructibleObject, IHitable, ICutable, IExplosive
{
    private readonly float _forceMultiplier = 3f;
    private WoodDestructibleAudioHandler _woodDestructibleAudioHandler;
    private IGlobalAudio _globalAudio;
    private AudioClipProvider _audioClipProvider;
    private TimeScaleSignal _timeScaleSignal;
    
    public Vector2 Position => TransformBase.position;
    public bool IsBroken => base.ObjectIsBroken;
    
    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

    [Inject]
    private void Construct(IGlobalAudio globalAudio, AudioClipProvider audioClipProvider, TimeScaleSignal timeScaleSignal)
    {
        _globalAudio = globalAudio;
        _audioClipProvider = audioClipProvider;
        _timeScaleSignal = timeScaleSignal;
    }

    private void Awake()
    {
        _woodDestructibleAudioHandler = new WoodDestructibleAudioHandler(GetComponent<AudioSource>(),
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty, new TimeScalePitchHandler(_timeScaleSignal),
            _audioClipProvider.LevelAudioClipProvider.WoodBreaking1AudioClip,
            _audioClipProvider.LevelAudioClipProvider.WoodBreaking2AudioClip,
            _audioClipProvider.LevelAudioClipProvider.HitWood1AudioClip,
            _audioClipProvider.LevelAudioClipProvider.HitWood2AudioClip,
            _audioClipProvider.LevelAudioClipProvider.HitWood3AudioClip);
        
        base.Init(_woodDestructibleAudioHandler.PlayHitWoodSound);
    }
    public bool TryBreakOnExplosion(Vector2 direction, float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (forceHit > Hardness)
            {
                _woodDestructibleAudioHandler.PlayWoodBreakingSound();
                Destruct();
                result = true;
            }
            else
            {
                Rigidbody2D.AddForce(direction * forceHit * _forceMultiplier);
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    public void DestructFromCut(Vector2 cutPos)
    {
        if (ObjectIsBroken == false)
        {
            _woodDestructibleAudioHandler.PlayWoodBreakingSound();
            Destruct();
        }
    }

    public bool TryBreakOnImpact(float forceHit)
    {
        bool result;
        if (IsBroken == false)
        {
            if (forceHit > Hardness)
            {
                _woodDestructibleAudioHandler.PlayWoodBreakingSound();
                Destruct();
                result = true;
            }
            else
            {
                _woodDestructibleAudioHandler.PlayWoodNotBreakingSound(forceHit);
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }

    private new void OnEnable()
    {
        base.OnEnable();
    }

    private new void OnDestroy()
    {
        _woodDestructibleAudioHandler?.Dispose();
        base.OnDestroy();
    }
}