using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelAudioClipProvider", menuName = "Providers/LevelAudioClipProvider", order = 51)]
public class LevelAudioClipProvider : ScriptableObject
{
    [SerializeField, BoxGroup("Wood")] private AudioClip _woodBreaking1AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _woodBreaking2AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _woodBreaking3AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _hitWood1AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _hitWood2AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _hitWood3AudioClip;
    
    [SerializeField, BoxGroup("Barrel"), HorizontalLine(color: EColor.White)] private AudioClip _hitBarrelAudioClip;
    [SerializeField, BoxGroup("Barrel")] private AudioClip _hitDebrisBarrelAudioClip;
    [SerializeField, BoxGroup("Barrel")] private AudioClip _explodeBarrelAudioClip;

    public AudioClip WoodBreaking1AudioClip => _woodBreaking1AudioClip;
    public AudioClip WoodBreaking2AudioClip => _woodBreaking2AudioClip;
    public AudioClip WoodBreaking3AudioClip => _woodBreaking3AudioClip;
    public AudioClip HitWood1AudioClip => _hitWood1AudioClip;
    public AudioClip HitWood2AudioClip => _hitWood2AudioClip;
    public AudioClip HitWood3AudioClip => _hitWood3AudioClip;
    
    public AudioClip HitBarrelAudioClip => _hitBarrelAudioClip;
    public AudioClip HitDebrisBarrelAudioClip => _hitDebrisBarrelAudioClip;
    public AudioClip ExplodeBarrelAudioClip => _explodeBarrelAudioClip;
}
