using UnityEngine;

public class SpeedEffectHandler
{
    private readonly float _minValue = 10f;
    private readonly float _maxValue = 70f;
    private readonly float _minSpeed = 15f;
    private readonly Speedometer _speedometer;
    private readonly SpeedEffect _effect;
    private float _alpha;

    public SpeedEffectHandler(Speedometer speedometer, SpeedEffect effect)
    {
        _speedometer = speedometer;
        _effect = effect;
    }

    public void Update()
    {
        _alpha = Mathf.InverseLerp(_minValue, _maxValue, _speedometer.CurrentSpeedFloat);
        if (_alpha > _minSpeed)
        {
            _effect.ChangeAlpha(_alpha);
            _effect.Play();
        }
        else
        {
            _effect.Stop();
        }
    }
}
