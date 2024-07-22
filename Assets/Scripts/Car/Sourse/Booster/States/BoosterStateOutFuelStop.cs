
using UnityEngine;

public class BoosterStateOutFuelStop : BoosterState
{
    public BoosterStateOutFuelStop(BoosterValues boosterValues, BoosterScrew boosterScrew, BoosterAudioHandler boosterAudioHandler, ParticleSystem particleSystemBooster)
        : base(boosterScrew, boosterAudioHandler, boosterValues, particleSystemBooster)
    {
        
    }
    public override void Enter()
    {
        BoosterAudioHandler.PlayBoosterEndFuel();
        ParticleSystemBooster.Stop();
    }
}