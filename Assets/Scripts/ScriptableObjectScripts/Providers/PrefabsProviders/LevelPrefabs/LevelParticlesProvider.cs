using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParticlesProvider", menuName = "LevelContent/LevelParticlesProvider", order = 51)]
public class LevelParticlesProvider : ScriptableObject
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private ParticleSystem _barrelExplosion;
    [SerializeField] private ParticleSystem _burnEffect;
    [SerializeField] private ParticleSystem _bloodEffect;
    [SerializeField] private ParticleSystem _smokeEffect;
    [SerializeField] private DebrisEffect _debrisEffect;


    public ParticleSystem BarrelExplosion => _barrelExplosion;
    public ParticleSystem BurnEffect => _burnEffect;
    public ParticleSystem SmokeEffect => _smokeEffect;
    public ParticleSystem BloodEffect => _bloodEffect;
    public DebrisEffect DebrisEffect => _debrisEffect;
}
