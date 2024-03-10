using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsUpgradeCar
{
    private Wallet _wallet;
    private AppearanceButtonsAnimation _appearanceButtonsAnimation;
    private DescriptionsButtonsUpgrades _descriptionsButtonsUpgrades;
    private ConfirmationUpgrade _confirmationUpgrade;
    private ButtonWithStatusBar _buttonEngineUpgrade;
    private ButtonWithStatusBar _buttonGearboxUpgrade;
    private ButtonWithStatusBar _buttonWheelsUpgrade;
    private ButtonWithStatusBar _buttonGunUpgrade;
    private ButtonWithStatusBar _buttonCorpusUpgrade;
    private ButtonWithStatusBar _buttonBoosterUpgrade;
    private ButtonWithStatusBar _buttonFueltankUpgrade;
    private ButtonWithStatusBar[] _buttonWithStatusBars;
    private UpgradeParkingLot _upgradeParkingLot;

    private UpgradeButton _upgradeButtonEngine;
    private UpgradeButton _upgradeButtonGearbox;
    private UpgradeButton _upgradeButtonWheels;
    private UpgradeButton _upgradeButtonGun;
    private UpgradeButton _upgradeButtonCorpus;
    private UpgradeButton _upgradeButtonBooster;
    private UpgradeButton _upgradeButtonFueltank;
    private UpgradeButton[] _upgradeButtons;

    private List<CanvasGroup> _canvasGroupsButtonsUpgrade;
    private IReadOnlyList<UpgradeButtonsGarageContent> _spritesLotsToButtons;
    private IReadOnlyList<Sprite> _upgradeButtonsSprites; 
    public ButtonsUpgradeCar(IReadOnlyList<UpgradeButtonsGarageContent> spritesLotsToButtons, GaragePanel garagePanel, Garage garage, ConfirmationUpgrade confirmationUpgrade, Spawner spawner, AudioHandlerUI audioHandlerUI, Image segmentIconPrefab, DescriptionsButtonsUpgrades descriptionsButtonsUpgrades)
    {
        _spritesLotsToButtons = spritesLotsToButtons;
        _descriptionsButtonsUpgrades = descriptionsButtonsUpgrades;
        _confirmationUpgrade = confirmationUpgrade;
        _wallet = garage.Wallet;
        _upgradeParkingLot = garage.UpgradeParkingLot;
        InitButtonsWithStatusBar(garagePanel);
        InitUpgradeButtons(garage, confirmationUpgrade, segmentIconPrefab, spawner, audioHandlerUI);
        _appearanceButtonsAnimation = new AppearanceButtonsAnimation(InitCanvasGroupForAnimation(), garagePanel.ParentRectTransformButtonsUpgrade);
    }

    public void ActivateButtons(ParkingLot parkingLot, int lotIndex)
    {
        _upgradeButtonsSprites = _spritesLotsToButtons[lotIndex].GetSpriteList();
        _confirmationUpgrade.OnUpgradePricesUpdate += SetPrices;
        _confirmationUpgrade.OnUpgradePricesUpdate += SetColorsPricesButtons;
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeButtons[i].Activate();
            _upgradeButtons[i].CreateIndicatorBar(
                parkingLot.ParkingLotConfiguration.GetMaxIndexesValues[i],
                parkingLot.ParkingLotConfiguration.GetCurrentIndexesValues[i]);
            _upgradeButtons[i].SetIcon(_upgradeButtonsSprites[i]);
        }
        SetPrices();
        SetColorsPricesButtons();
        _appearanceButtonsAnimation.Unhide();
    }
    public async void DeactivateButtons()
    {
        _confirmationUpgrade.OnUpgradePricesUpdate -= SetPrices;
        _confirmationUpgrade.OnUpgradePricesUpdate -= SetColorsPricesButtons;

        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeButtons[i].Deactivate();
        }
        await _appearanceButtonsAnimation.Hide();
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeButtons[i].ResetIndicator();
            _upgradeButtons[i].ResetIcon();
        }
    }

    public void DeactivateButtonsOnLoadGame()
    {
        _confirmationUpgrade.OnUpgradePricesUpdate -= SetPrices;
        _confirmationUpgrade.OnUpgradePricesUpdate -= SetColorsPricesButtons;
    }
    private void SetPrices()
    {
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeButtons[i].SetPrice(_upgradeParkingLot.ParkingLotConfiguration.GetPricesString[i]);
        }
    }
    private void SetColorsPricesButtons()
    {
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            if (_wallet.CheckAvailableMoney(_upgradeParkingLot.ParkingLotConfiguration.GetPricesUpgradeForColors[i]) == true)
            {
                _upgradeButtons[i].SetColorSufficientAmountMoneyToBuy();
            }
            else
            {
                _upgradeButtons[i].SetColorNotSufficientAmountMoneyToBuy();
            }
        }
    }
    private void InitButtonsWithStatusBar(GaragePanel garagePanel)
    {
        _buttonEngineUpgrade = garagePanel.ButtonEngineUpgrade;
        _buttonGearboxUpgrade = garagePanel.ButtonGearboxUpgrade;
        _buttonWheelsUpgrade = garagePanel.ButtonWheelsUpgrade;
        _buttonGunUpgrade = garagePanel.ButtonGunUpgrade;
        _buttonCorpusUpgrade = garagePanel.ButtonCorpusUpgrade;
        _buttonBoosterUpgrade = garagePanel.ButtonBoosterUpgrade;
        _buttonFueltankUpgrade = garagePanel.ButtonFueltankUpgrade;
        _buttonWithStatusBars = new ButtonWithStatusBar[]
        {
            garagePanel.ButtonEngineUpgrade,
            garagePanel.ButtonGearboxUpgrade,
            garagePanel.ButtonWheelsUpgrade,
            garagePanel.ButtonGunUpgrade,
            garagePanel.ButtonCorpusUpgrade,
            garagePanel.ButtonBoosterUpgrade,
            garagePanel.ButtonFueltankUpgrade
        };
    }
    private void InitUpgradeButtons(Garage garage, ConfirmationUpgrade confirmationUpgrade, Image segmentIconPrefab, Spawner spawner, AudioHandlerUI audioHandlerUI)
    {
        _upgradeButtons = new UpgradeButton[] 
        {
            _upgradeButtonEngine = new UpgradeButton(_buttonEngineUpgrade, confirmationUpgrade, audioHandlerUI, garage.UpgradeParkingLot.EngineUpgrade, segmentIconPrefab, spawner, _descriptionsButtonsUpgrades.GetNameEngineUpgrade, _descriptionsButtonsUpgrades.GetDescriptionEngineUpgrade),
            _upgradeButtonGearbox = new UpgradeButton(_buttonGearboxUpgrade, confirmationUpgrade, audioHandlerUI, garage.UpgradeParkingLot.GearboxUpgrade, segmentIconPrefab, spawner, _descriptionsButtonsUpgrades.GetNameGearboxUpgrade, _descriptionsButtonsUpgrades.GetDescriptionGearboxUpgrade),
            _upgradeButtonWheels = new UpgradeButton(_buttonWheelsUpgrade, confirmationUpgrade, audioHandlerUI, garage.UpgradeParkingLot.WheelUpgrade, segmentIconPrefab, spawner, _descriptionsButtonsUpgrades.GetNameWheelUpgrade, _descriptionsButtonsUpgrades.GetDescriptionWheelUpgrade),
            _upgradeButtonGun = new UpgradeButton(_buttonGunUpgrade, confirmationUpgrade, audioHandlerUI, garage.UpgradeParkingLot.GunUpgrade, segmentIconPrefab, spawner, _descriptionsButtonsUpgrades.GetNameGunUpgrade, _descriptionsButtonsUpgrades.GetDescriptionGunUpgrade),
            _upgradeButtonCorpus = new UpgradeButton(_buttonCorpusUpgrade, confirmationUpgrade, audioHandlerUI, garage.UpgradeParkingLot.CorpusUpgrade, segmentIconPrefab, spawner, _descriptionsButtonsUpgrades.GetNameCorpusUpgrade, _descriptionsButtonsUpgrades.GetDescriptionCorpusUpgrade),
            _upgradeButtonBooster = new UpgradeButton(_buttonBoosterUpgrade, confirmationUpgrade, audioHandlerUI, garage.UpgradeParkingLot.BoosterUpgrade, segmentIconPrefab, spawner, _descriptionsButtonsUpgrades.GetNameBoosterUpgrade, _descriptionsButtonsUpgrades.GetDescriptionBoosterUpgrade),
            _upgradeButtonFueltank = new UpgradeButton(_buttonFueltankUpgrade, confirmationUpgrade, audioHandlerUI, garage.UpgradeParkingLot.FueltankUpgrade, segmentIconPrefab, spawner, _descriptionsButtonsUpgrades.GetNameTankUpgrade, _descriptionsButtonsUpgrades.GetDescriptionTankUpgrade)
        };
    }
    private IReadOnlyList<CanvasGroup> InitCanvasGroupForAnimation()
    {
        _canvasGroupsButtonsUpgrade = new List<CanvasGroup>(_upgradeButtons.Length);
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _canvasGroupsButtonsUpgrade.Add(_buttonWithStatusBars[i].CanvasGroup);
        }
        return _canvasGroupsButtonsUpgrade;
    }
}
