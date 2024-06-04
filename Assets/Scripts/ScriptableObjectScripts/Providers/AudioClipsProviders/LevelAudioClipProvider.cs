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
    
    [SerializeField, BoxGroup("Zommbie"), HorizontalLine(color: EColor.Green)] private AudioClip _hit1ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _hit2ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _death1ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _death2ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _death3ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _death4ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _death5ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _talk1ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _talk2ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _talk3ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _talk4ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _talk5ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _talk6ZombieAudioClip;
    [SerializeField, BoxGroup("Zommbie")] private AudioClip _fartZombieAudioClip;


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
    public AudioClip Hit1ZombieAudioClip => _hit1ZombieAudioClip;
    public AudioClip Hit2ZombieAudioClip => _hit2ZombieAudioClip;
    public AudioClip Death1ZombieAudioClip => _death1ZombieAudioClip;
    public AudioClip Death2ZombieAudioClip => _death2ZombieAudioClip;
    public AudioClip Death3ZombieAudioClip => _death3ZombieAudioClip;
    public AudioClip Death4ZombieAudioClip => _death4ZombieAudioClip;
    public AudioClip Death5ZombieAudioClip => _death5ZombieAudioClip;
    
    public AudioClip Talk1ZombieAudioClip => _talk1ZombieAudioClip;
    public AudioClip Talk2ZombieAudioClip => _talk2ZombieAudioClip;
    public AudioClip Talk3ZombieAudioClip => _talk3ZombieAudioClip;
    public AudioClip Talk4ZombieAudioClip => _talk4ZombieAudioClip;
    public AudioClip Talk5ZombieAudioClip => _talk5ZombieAudioClip;
    public AudioClip Talk6ZombieAudioClip => _talk6ZombieAudioClip;
    
    public AudioClip FartZombieAudioClip => _fartZombieAudioClip;
}
