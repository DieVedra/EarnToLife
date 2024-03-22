using UnityEngine;

public class ExhaustHandler
{
    private readonly Transform _point1;
    private readonly Transform _point2;
    private readonly Transform _point3;
    private readonly ParticleSystem _exhaustParticleSystem;
    public ExhaustHandler(ParticleSystem exhaustParticleSystem, Transform point1, Transform point2, Transform point3)
    {
        _point1 = point1;
        _point2 = point2;
        _point3 = point3;
        _exhaustParticleSystem = exhaustParticleSystem;
        SetPoint1();
        _exhaustParticleSystem.Play();
    }

    private void SetPoint1()
    {
        _exhaustParticleSystem.transform.position = _point1.transform.position;
    }
    public void SetPoint2()
    {
        _exhaustParticleSystem.transform.position = _point2.transform.position;
    }
    public void SetPoint3()
    {
        _exhaustParticleSystem.transform.position = _point3.transform.position;
    }
}