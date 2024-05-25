
using UnityEngine;

public class ZombiePool
{
    private readonly ParticleSystem _bloodEffect;
    private readonly Factory _factory;
    private readonly Transform _bloodEffectsPoolParent;

    public ZombiePool(ParticleSystem bloodEffect, Factory factory, Transform bloodEffectsPoolParent)
    {
        _bloodEffect = bloodEffect;
        _factory = factory;
        _bloodEffectsPoolParent = bloodEffectsPoolParent;
    }
}