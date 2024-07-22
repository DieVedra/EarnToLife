
using UnityEngine;

public class BoosterState
{
    protected readonly BoosterValues BoosterValues;
    protected readonly ParticleSystem ParticleSystemBooster;
    protected readonly BoosterScrew BoosterScrew;
    protected readonly BoosterAudioHandler BoosterAudioHandler;
    protected BoosterState(BoosterScrew boosterScrew, BoosterAudioHandler boosterAudioHandler, BoosterValues boosterValues, ParticleSystem particleSystemBooster)
    {
        BoosterScrew = boosterScrew;
        BoosterAudioHandler = boosterAudioHandler;
        BoosterValues = boosterValues;
        ParticleSystemBooster = particleSystemBooster;
    }
    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }
}