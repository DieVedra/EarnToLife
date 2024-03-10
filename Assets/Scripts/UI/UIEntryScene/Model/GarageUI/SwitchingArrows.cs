using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchingArrows
{
    private ButtonsUpgradeCar _buttonsUpgradeCar;
    private Garage _garage;
    private Button _buttonRight;
    private Button _buttonLeft;
    private Image _imageButtonRight;
    private Image _imageButtonLeft;
    private LockLotIndicator _lockLotIndicator;
    private SwitchGarageLot _switchGarageLot;
    private AnimationSwitchArrows _animationSwitchArrows;
    private AudioHandlerUI _audioHandlerUI;
   
    public SwitchingArrows(ButtonsUpgradeCar buttonsUpgradeCar, Garage garage, Button buttonRight, Button buttonLeft, LockLotIndicator lockLotIndicator, AudioHandlerUI audioHandlerUI)
    {
        _audioHandlerUI = audioHandlerUI;
        _animationSwitchArrows = new AnimationSwitchArrows(buttonRight.GetComponent<Image>(), buttonLeft.GetComponent<Image>());
        _buttonsUpgradeCar = buttonsUpgradeCar;
        _garage = garage;
        _buttonRight = buttonRight;
        _buttonLeft = buttonLeft;
        _lockLotIndicator = lockLotIndicator;
        _switchGarageLot = _garage.SwitchGarageLot;
        _imageButtonRight = _buttonRight.GetComponent<Image>();
        _imageButtonLeft = _buttonLeft.GetComponent<Image>();
    }

    public void Activate()
    {
        SubscribeButtonsSwitch();
        _lockLotIndicator.CheckStatus();
        CheckActiveButtons();
        _switchGarageLot.OnEndLotSwitch += LotEndSwitch;
    }
    public void Deactivate()
    {
        UnsubscribeButtonsSwitch();
        _switchGarageLot.OnEndLotSwitch -= LotEndSwitch;
    }

    private async void SwitchRight()
    {
        _audioHandlerUI.PlayClick();
        _switchGarageLot.SwitchLotRight();
        _lockLotIndicator.CheckStatus();
        _buttonsUpgradeCar.DeactivateButtons();
        ButtonsNotCanPush();
        await _animationSwitchArrows.SwitchRight();
        CheckActiveButtons();
    }

    private async void SwitchLeft()
    {
        _audioHandlerUI.PlayClick();
        _switchGarageLot.SwitchLotLeft();
        _lockLotIndicator.CheckStatus();
        _buttonsUpgradeCar.DeactivateButtons();
        ButtonsNotCanPush();
        await _animationSwitchArrows.SwitchLeft();
        CheckActiveButtons();
    }
    private void CheckActiveButtons()
    {
        DisableArrows();
        if (_garage.CurrentSelectLotCarIndex == 0)
        {
            FirstLotArrowsState();
        }
        else if (_garage.CurrentSelectLotCarIndex == _garage.ParkingLots.Count - 1)
        {
            EndLotArrowsState();
        }
        else
        {
            MiddleLotsArrowsState();
        }
    }
    private void ButtonsNotCanPush()
    {
        _buttonLeft.interactable = false;
        _buttonRight.interactable = false;
    }
    private void ButtonsCanPush()
    {
        _buttonLeft.interactable = true;
        _buttonRight.interactable = true;
    }
    private void SubscribeButtonsSwitch()
    {
        _buttonRight.onClick.AddListener(SwitchRight);
        _buttonLeft.onClick.AddListener(SwitchLeft);
    }
    private void UnsubscribeButtonsSwitch()
    {
        _buttonRight.onClick.RemoveAllListeners();
        _buttonLeft.onClick.RemoveAllListeners();
    }
    private void LotEndSwitch(ParkingLot parkingLot, int lotIndex)
    {
        if (parkingLot.IsBlocked == false)
        {
            _buttonsUpgradeCar.ActivateButtons(parkingLot, lotIndex);
        }
        ButtonsCanPush();
    }

    private void FirstLotArrowsState()
    {
        EnableRightArrow();
        _animationSwitchArrows.UnfadeArrowRight();;
    }
    private void MiddleLotsArrowsState()
    {
        if (_buttonLeft.gameObject.activeSelf == false)
        {
            EnableLeftArrow();
            _animationSwitchArrows.UnfadeArrowLeft();
        }
        if (_buttonRight.gameObject.activeSelf == false)
        {
            EnableRightArrow();
            _animationSwitchArrows.UnfadeArrowRight();
        }
    }
    private void EndLotArrowsState()
    {
        EnableLeftArrow();
        _animationSwitchArrows.UnfadeArrowLeft();
    }

    private void DisableArrows()
    {
        _buttonRight.gameObject.SetActive(false);
        _buttonLeft.gameObject.SetActive(false);
    }

    private void EnableRightArrow()
    {
        _buttonRight.gameObject.SetActive(true);
    }

    private void EnableLeftArrow()
    {
        _buttonLeft.gameObject.SetActive(true);
    }
}
