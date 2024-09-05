using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Box : DestructibleObject, IHitable, ICutable, IExplosive
{
    private readonly float _forceMultiplier = 3f;
    private WoodDestructibleAudioHandler _woodDestructibleAudioHandler;

    public Vector2 Position => TransformBase.position;

    public bool IsBroken => base.ObjectIsBroken;


    public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

    [Inject]
    private void Construct(ILevel level, IGlobalAudio globalAudio, AudioClipProvider audioClipProvider, TimeScaleSignal timeScaleSignal)
    {
        _woodDestructibleAudioHandler = new WoodDestructibleAudioHandler(GetComponent<AudioSource>(),
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty, new TimeScalePitchHandler(timeScaleSignal),
            audioClipProvider.LevelAudioClipProvider.WoodBreaking1AudioClip,
            audioClipProvider.LevelAudioClipProvider.WoodBreaking2AudioClip,
            audioClipProvider.LevelAudioClipProvider.HitWood1AudioClip,
            audioClipProvider.LevelAudioClipProvider.HitWood2AudioClip,
            audioClipProvider.LevelAudioClipProvider.HitWood3AudioClip);
        DebrisHitSound = _woodDestructibleAudioHandler.PlayHitWoodSound;
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