
using UnityEngine;

public class BoosterStateOnBrokenStop : BoosterState
{
    public BoosterStateOnBrokenStop(BoosterValues boosterValues, BoosterScrew boosterScrew, BoosterAudioHandler boosterAudioHandler, ParticleSystem particleSystemBooster)
        : base(boosterScrew, boosterAudioHandler, boosterValues, particleSystemBooster)
    {
        
    }

    public override void Enter()
    {
        ParticleSystemBooster.Stop();
        BoosterAudioHandler.StopPlayRunBoosterImmediately();
    }
}