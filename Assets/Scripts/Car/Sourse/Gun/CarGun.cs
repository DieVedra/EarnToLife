using System;
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
    private Transform _visionPointTransform;
    private CarGunTarget _currentTarget;
    private ParticleSystem _particleSystemShoot;
    private int _currentIndexTarget;
    private float _speedLook;
    private bool _isShooting = false;
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
    }
    public void Update()
    {
        if (_currentTarget is null)
        {
            if (_detector.TryFindTarget() == true)
            {
                SetCurrentTargetOnIndex();
                GuidanceToTarget();
            }
            else
            {
                GuidanceDefault();
            }
        }
        else
        {
            if (_detector.CheckRelevanceTarget(_currentTarget.Target) == false)
            {
                LoseCurrentTarget();
                GuidanceDefault();
            }
            else
            {
                GuidanceToTarget();
            }
        }
        if (_isShooting == true && _rateFire.CanShoot == true)
        {
            Shoot();
        }
    }
    private void GuidanceDefault()
    {
        Guidance(_defaultPointAiming, Vector3.zero, false);
    }
    private void GuidanceToTarget()
    {
        Guidance(_currentTarget.Target.TargetTransform, _currentTarget.Target.TargetTransform.position, true);
    }
    private void Guidance(Transform carGunTarget, Vector3 position, bool key)
    {
        _gunGuidance.Update(carGunTarget);
        OnUpdateTargetTracking?.Invoke(key, position);
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
            Debug.Log(11111111111111);

            Debug.Log($"CurrentIndex:  {_currentIndexTarget}");

            if (_currentIndexTarget > _detector.GetMaxIndexTargets)
            {
                _currentIndexTarget = 0;

            }
            else if (_currentIndexTarget < _detector.GetMaxIndexTargets)
            {
                Debug.Log($"MaxIndexTargets {_detector.GetMaxIndexTargets} > {_currentIndexTarget} currentIndexTarget  Index++");

                _currentIndexTarget++;
            }
            else if (_currentIndexTarget == _detector.GetMaxIndexTargets)
            {
                Debug.Log($"MaxIndexTargets {_detector.GetMaxIndexTargets} == {_currentIndexTarget} currentIndexTarget Index 0");
            
                _currentIndexTarget = 0;
            }
            SetCurrentTargetOnIndex();
        }
        
        
        // if (_detector.GetCurrentCountTargetsAfterSorted > 0)
        // {
        //     if (_detector.GetCurrentCountTargetsAfterSorted - 1 > _currentIndexTarget)
        //     {
        //         _currentIndexTarget++;
        //         SetCurrentTargetOnIndex(_currentIndexTarget);
        //     }
        //     else if (_detector.GetCurrentCountTargetsAfterSorted - 1 == _currentIndexTarget)
        //     {
        //         _currentIndexTarget = 0;
        //         SetCurrentTargetOnIndex(_currentIndexTarget);
        //     }
        // }
    }

    public void GunDisableFromDestruct()
    {
        OnGunDestruct?.Invoke();
    }
    // private bool TrySetCurrentTarget()
    // {
    //     if (_detector.TryFindTarget() == true)
    //     {
    //         SetCurrentTargetOnIndex(_currentIndexTarget);
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }
    private void SetCurrentTargetOnIndex()
    {
        Debug.Log($"CountTargets: {_detector.GetCurrentCountTargets}");
        Debug.Log($"_currentIndexTarget: {_currentIndexTarget}");

        Debug.Log(2222222222222);

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
        _gunGuidance.IsGuidanence = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_timeFreezeRotationAfterShoot));
        _gunGuidance.IsGuidanence = true;
    }
}