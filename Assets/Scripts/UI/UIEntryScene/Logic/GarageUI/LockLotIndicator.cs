using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LockLotIndicator
{
    private const float DURATION_FADE = 0.5f;
    private const float VALUE_TRANSPARENT = 0f;
    private const float VALUE_VISIBLE = 1f;
    private Image _imageLock;
    private Garage _garage;
    private bool _isActive;
    private bool _isCurrentParkingLotBlockStatus => _garage.ParkingLots[_garage.CurrentSelectLotCarIndex].IsBlocked;
    public LockLotIndicator(Image image, Garage garage)
    {
        _imageLock = image;
        _garage = garage;
        _isActive = false;
    }
    public void CheckStatus()
    {
        if (_isCurrentParkingLotBlockStatus == true)
        {
            TryActivate();
        }
        else
        {
            TryDeactivate();
        }
    }
    private async void TryActivate()
    {
        if (_isActive == false)
        {
            _imageLock.color = SetAlphaOff();
            _imageLock.gameObject.SetActive(true);
            _isActive = true;
            await _imageLock.DOFade(VALUE_VISIBLE, DURATION_FADE).SetEase(Ease.InQuint).ToUniTask();
        }
    }
    private async void TryDeactivate()
    {
        if (_isActive == true)
        {
            _imageLock.gameObject.SetActive(false);
            _isActive = false;
            await _imageLock.DOFade(VALUE_TRANSPARENT, DURATION_FADE).SetEase(Ease.OutQuint).ToUniTask();
        }
    }
    private Color SetAlphaOff()
    {
        return new Color(_imageLock.color.r, _imageLock.color.g, _imageLock.color.b, VALUE_TRANSPARENT);
    }
}
