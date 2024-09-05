using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CarGun
{
    private readonly float _timeFreezeRotationAfterShoot = 0.2f;
    private readonly float _forceGunValue;
    private readonly Transform _defaultPointAiming;
    private readonly GunAudioHandler _gunAudioHandler;
    private readonly RateFire _rateFire;
    private readonly GunGuidance _gunGuidance;
    private readonly CarGunDetector _detector;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private Transform _visionPointTransform;
    private CarGunTarget _currentTarget;
    private ParticleSystem _particleSystemShoot;
    private Vector3 _positionCrosshair;
    private int _currentIndexTarget;
    private float _speedLook;
    private bool _isShooting = false;
    private bool _isShowCrosshair = false;
    public int Ammo { get; private set; }
    public event Action<bool, Vector3> OnUpdateTargetTracking;
    public event Action<int> OnAmmoUpdate;
    public event Action OnGunDestruct;
    
    public CarGun(Transform defaultPointAiming,
        GunAudioHandler gunAudioHandler, CarGunDetector carGunDetector, GunGuidance gunGuidance, ParticleSystem particleSystemShoot,
        int ammo, float rateFireValue, float forceGunValue)
    {
        _defaultPointAiming = defaultPointAiming;
        _gunAudioHandler = gunAudioHandler;
        _rateFire = new RateFire(rateFireValue);
        _detector = carGunDetector;
        _gunGuidance = gunGuidance;
        _particleSystemShoot = particleSystemShoot;
        Ammo = ammo;
        _forceGunValue = forceGunValue;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public void Update()
    {
        if (Ammo > 0)
        {
            if (_currentTarget is null)
            {
                if (_detector.TryFindTarget() == true)
                {
                    SetCurrentTargetOnIndex();
                    SetGuidanceToTarget();
                }
                else
                {
                    SetGuidanceDefault();
                }
            }
            else
            {
                if (_detector.CheckRelevanceTarget(_currentTarget.Target) == false)
                {
                    LoseCurrentTarget();
                    SetGuidanceDefault();
                }
                else
                {
                    SetGuidanceToTarget();
                }
            }

            if (_currentTarget is null)
            {
                _gunGuidance.Update(_defaultPointAiming);
            }
            else
            {
                _gunGuidance.Update(_currentTarget.Target.TargetTransform);
            }

            TargetTracking();
            if (_isShooting == true && _rateFire.CanShoot == true)
            {
                Shoot();
            }
        }
        else
        {
            _gunGuidance.Update(_defaultPointAiming);
        }
    }
    private void SetGuidanceDefault()
    {
        _positionCrosshair = _defaultPointAiming.position;
        _isShowCrosshair = false;
    }
    private void SetGuidanceToTarget()
    {
        _positionCrosshair = _currentTarget.Target.TargetTransform.position;
        _isShowCrosshair = true;
    }
    private void Shoot()
    {
        if (_currentTarget != null && Ammo > 0)
        {
            _particleSystemShoot.Play();
            _gunAudioHandler.PlayShotGun();
            _currentTarget.Target.DestructFromShoot(CalculateShootDirectionForce());
            Ammo--;
            OnAmmoUpdate?.Invoke(Ammo);
            _rateFire.StartRateFireDelay();
            FreezeRotationAfterShoot().Forget();
        }
    }
    public void SetShootKey(bool key)
    {
        _isShooting = key;
    }
    public void ChangeIndex()
    {
        if (_detector.TryFindTarget() == true)
        {
            if (_currentIndexTarget > _detector.GetMaxIndexTargets)
            {
                _currentIndexTarget = 0;

            }
            else if (_currentIndexTarget < _detector.GetMaxIndexTargets)
            {
                _currentIndexTarget++;
            }
            else if (_currentIndexTarget == _detector.GetMaxIndexTargets)
            {
                _currentIndexTarget = 0;
            }
            SetCurrentTargetOnIndex();
        }
    }

    public void GunDisableFromDestruct()
    {
        OnGunDestruct?.Invoke();
    }

    private void TargetTracking()
    {
        OnUpdateTargetTracking?.Invoke(_isShowCrosshair, _positionCrosshair);
    }
    private void SetCurrentTargetOnIndex()
    {
        _currentTarget = _detector.GetTarget(_currentIndexTarget);
    }
    private void LoseCurrentTarget()
    {
        _currentTarget = null;
    }
    private Vector2 CalculateShootDirectionForce()
    {
        return (_currentTarget.Target.TargetTransform.position - _particleSystemShoot.transform.position).normalized * _forceGunValue;
    }
    private async UniTask FreezeRotationAfterShoot()
    {
        _gunGuidance.FreezedGuidanenceAfterShoot = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_timeFreezeRotationAfterShoot), cancellationToken:_cancellationTokenSource.Token);
        _gunGuidance.FreezedGuidanenceAfterShoot = false;
    }
}