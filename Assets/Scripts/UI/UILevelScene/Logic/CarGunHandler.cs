using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UniRx;

public class CarGunHandler
{
    private readonly CustomButton _fireButton;
    private readonly Button _buttonSkipTargetOnShooting;
    private readonly Transform _transformTargetOnShooting;
    private readonly TextMeshProUGUI _ammoIndicator;
    private readonly TextMeshProUGUI _textAmmoIndicator;
    private readonly CarGun _carGun;
    private int _ammo;
    public CarGunHandler(ViewUILevel viewUILevel, CarGun carGun)
    {
        _fireButton = viewUILevel.ButtonFire;
        _ammoIndicator = viewUILevel.DevicePanel.AmmoIndicator;
        _textAmmoIndicator = _ammoIndicator.GetComponentInChildren<TextMeshProUGUI>();
        _buttonSkipTargetOnShooting = viewUILevel.ButtonCrosshair;
        _transformTargetOnShooting = viewUILevel.ButtonCrosshair.transform;
        _carGun = carGun;
        _carGun.OnUpdateTargetTracking += TargetTracking;
        _carGun.OnAmmoUpdate += AmmoUpdate;
        _carGun.OnGunDestruct += GunDestruct;
        _fireButton.OnButtonDown += SetKeyTrue;
        _fireButton.OnButtonUp += SetKeyFalse;
        _buttonSkipTargetOnShooting.onClick.AddListener(_carGun.ChangeIndex);
        _transformTargetOnShooting.gameObject.SetActive(false);
        _ammoIndicator.gameObject.SetActive(true);
        AmmoUpdate(carGun.Ammo);
        viewUILevel.DisposeCommand.Subscribe(_ => { Dispose();});
    }
    private void TargetTracking(bool targetActive, Vector3 point)
    {
        if (targetActive == true)
        {
            _transformTargetOnShooting.position = point;
            _transformTargetOnShooting.gameObject.SetActive(true);
        }
        else
        {
            _transformTargetOnShooting.gameObject.SetActive(false);
        }
    }
    private void AmmoUpdate(int count)
    {
        _textAmmoIndicator.text = count.ToString();
    }
    private void SetKeyTrue()
    {
        if (_fireButton.interactable == true)
        {
            _carGun.SetShootKey(true);
        }
    }
    private void SetKeyFalse()
    {
        _carGun.SetShootKey(false);
    }

    private void Dispose()
    {
        _carGun.OnUpdateTargetTracking -= TargetTracking;
        _carGun.OnAmmoUpdate -= AmmoUpdate;
        _carGun.OnGunDestruct -= GunDestruct;
        _fireButton.OnButtonDown -= SetKeyTrue;
        _fireButton.OnButtonUp -= SetKeyFalse;
        _buttonSkipTargetOnShooting.onClick.RemoveAllListeners();
    }
    private void GunDestruct()
    {
        AmmoUpdate(0);
        _transformTargetOnShooting.gameObject.SetActive(false);
        _fireButton.interactable = false;
        Dispose();
    }
}
