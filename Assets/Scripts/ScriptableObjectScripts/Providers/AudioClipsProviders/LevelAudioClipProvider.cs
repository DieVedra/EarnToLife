using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelAudioClipProvider", menuName = "Providers/LevelAudioClipProvider", order = 51)]
public class LevelAudioClipProvider : ScriptableObject
{
    [SerializeField, BoxGroup("Wood"), HorizontalLine(color: EColor.Yellow)] private AudioClip _woodBreaking1AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _woodBreaking2AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _woodBreaking3AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _hitWood1AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _hitWood2AudioClip;
    [SerializeField, BoxGroup("Wood")] private AudioClip _hitWood3AudioClip;
    
    [SerializeField, BoxGroup("Barrel"), HorizontalLine(color: EColor.Red)] private AudioClip _hitBarrelAudioClip;
    [SerializeField, BoxGroup("Barrel")] private AudioClip _hit1DebrisBarrelAudioClip;
    [SerializeField, BoxGroup("Barrel")] private AudioClip _hit2DebrisBarrelAudioClip;
    [SerializeField, BoxGroup("Barrel")] private AudioClip _explode1BarrelAudioClip;
    [SerializeField, BoxGroup("Barrel")] private AudioClip _explode2BarrelAudioClip;
    [SerializeField, BoxGroup("Barrel")] private AudioClip _burnBarrelAudioClip;
    
    [SerializeField, BoxGroup("Zommbie"), HorizontalLine(color: EColor.Green)] private AudioClip _hitZombieRemainsAudioClip;


    public AudioClip WoodBreaking1AudioClip => _woodBreaking1AudioClip;
    public AudioClip WoodBreaking2AudioClip => _woodBreaking2AudioClip;
    public AudioClip WoodBreaking3AudioClip => _woodBreaking3AudioClip;
    public AudioClip HitWood1AudioClip => _hitWood1AudioClip;
    public AudioClip HitWood2AudioClip => _hitWood2AudioClip;
    public AudioClip HitWood3AudioClip => _hitWood3AudioClip;
    
    public AudioClip HitBarrelAudioClip => _hitBarrelAudioClip;
    public AudioClip Hit1DebrisBarrelAudioClip => _hit1DebrisBarrelAudioClip;
    public AudioClip Hit2DebrisBarrelAudioClip => _hit2DebrisBarrelAudioClip;
    public AudioClip Explode1BarrelAudioClip => _explode1BarrelAudioClip;
    public AudioClip Explode2BarrelAudioClip => _explode2BarrelAudioClip;
    public AudioClip BurnBarrelAudioClip => _burnBarrelAudioClip;
    public AudioClip HitZombieRemainsAudioClip => _hitZombieRemainsAudioClip;
}
