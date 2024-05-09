using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParticlesProvider", menuName = "LevelContent/LevelParticlesProvider", order = 51)]
public class LevelParticlesProvider : ScriptableObject
{
    [SerializeField, BoxGroup("Barrel"), HorizontalLine(color: EColor.Green)] private ParticleSystem _barrelExplosion;
    [SerializeField, BoxGroup("Barrel")] private ParticleSystem _barrelBurnEffect;
    [SerializeField, BoxGroup("Barrel")] private DebrisBarrelEffect _debrisBarrelEffect;

    
    public ParticleSystem BarrelExplosion => _barrelExplosion;
    public ParticleSystem BarrelBurnEffect => _barrelBurnEffect;
    public DebrisBarrelEffect DebrisBarrelEffect => _debrisBarrelEffect;
}
