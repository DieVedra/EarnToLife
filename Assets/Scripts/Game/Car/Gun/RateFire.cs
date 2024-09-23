using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RateFire
{
    private readonly float _maxTime;
    private float _currentCountTime = 0f;
    // public event Action DoFire;
    public bool CanShoot { get; private set; } = true;
    public RateFire(float maxTime)
    {
        _maxTime = maxTime;
    }

    public async void StartRateFireDelay()
    {
        CanShoot = false;
        _currentCountTime = _maxTime;
        while (_currentCountTime > 0f)
        {
            _currentCountTime -= Time.deltaTime;
            await UniTask.Yield();
        }
        CanShoot = true;
    }
    // public void UpdateActivityFromList(bool canShoot)
    // {
    //     if (_currentCountTime > 0f)
    //     {
    //         _currentCountTime -= Time.deltaTime;
    //     }
    //     else if (canShoot == false)
    //     {
    //         return;
    //     }
    //     else if (_currentCountTime <= 0f)
    //     {
    //         // DoFire?.Invoke();
    //         _currentCountTime = _maxTime;
    //     }
    // }
}
