using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpeedEffect : MonoBehaviour
{
    [SerializeField, HorizontalLine(color: EColor.Black)] private ParticleSystem _particleParent;
    [SerializeField] private ParticleSystem _particleChild1;
    [SerializeField] private ParticleSystem _particleChild2;
    [SerializeField] private ParticleSystem _particleChild3;
    private ParticleSystem.MainModule[] _mainModules;
    private Color _color;

    public void ChangeAlpha(float alpha)
    {
        _color = new Color(1f,1f,1f,alpha);
        for (int i = 0; i < _mainModules.Length; i++)
        {
            _mainModules[i].startColor = _color;
        }
    }

    public void Play()
    {
        _particleParent.Play();
    }

    public void Stop()
    {
        _particleParent.Stop();
    }
    private void Start()
    {
        _mainModules = new[]
        {
            _particleParent.main,
            _particleChild1.main,
            _particleChild2.main,
            _particleChild3.main
        };
    }
}
