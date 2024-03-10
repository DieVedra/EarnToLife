using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GarageUI
{
    private Garage _garage;
    private ButtonsUpgradeCar _buttonsUpgradeCar;
    private SwitchingArrows _switchingArrows;
    private Image _imageButtonRight;
    private Image _imageButtonLeft;
    private TextMeshProUGUI _cash;
    private ParkingLot _currentParkingLot => _garage.ParkingLots[_garage.CurrentSelectLotCarIndex];
    public GarageUI(GaragePanel garagePanel, Garage garage, ButtonsUpgradeCar buttonsUpgradeCar, AudioHandlerUI audioHandlerUI)
    {
        _garage = garage;
        _buttonsUpgradeCar = buttonsUpgradeCar;
        _cash = garagePanel.Cash;
        _switchingArrows = new SwitchingArrows(buttonsUpgradeCar, garage, garagePanel.ButtonSlideRight, garagePanel.ButtonSlideLeft, new LockLotIndicator(garagePanel.Lock, garage), audioHandlerUI);
    }
    public void Activate()
    {
        _garage.Wallet.OnTakeCashSuccess += DisplayCashValue;
        _garage.Wallet.OnAddCashSuccess += DisplayCashValue;
        _garage.Activate();
        _buttonsUpgradeCar.ActivateButtons(_currentParkingLot, _garage.CurrentSelectLotCarIndex);
        _switchingArrows.Activate();
        DisplayCashValue(_garage.Wallet.Money);
    }
    public void Deactivate()
    {
        _garage.Deactivate();
        _buttonsUpgradeCar.DeactivateButtons();
        _switchingArrows.Deactivate();
        _garage.Wallet.OnTakeCashSuccess -= DisplayCashValue;
        _garage.Wallet.OnAddCashSuccess -= DisplayCashValue;
    }

    public void DeactivateOnLoad()
    {
        _garage.Deactivate();
        _buttonsUpgradeCar.DeactivateButtonsOnLoadGame();
        _switchingArrows.Deactivate();
        _garage.Wallet.OnTakeCashSuccess -= DisplayCashValue;
        _garage.Wallet.OnAddCashSuccess -= DisplayCashValue;
    }
    private void DisplayCashValue(int cash)
    {
        _cash.text = cash.ToString();
    }
}