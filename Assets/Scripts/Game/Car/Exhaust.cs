using UnityEngine;

public class Exhaust
{
    private readonly ParticleSystem _effect;
    public Transform EffectTransform => _effect.transform;
    public Exhaust(ParticleSystem effect)
    {
        _effect = effect;
    }

    public void SetSmokeSpeed(float value)
    {
        var mainModule = _effect.main;
        mainModule.startSpeed = value;
    }

    public void StopPlayEffect()
    {
        _effect.Stop();
    }
}