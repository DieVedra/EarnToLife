using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeParkingLot
{
    private Wallet _wallet;
    private ParkingLot _currentParkingLot;
    public ParkingLotConfiguration ParkingLotConfiguration { get; private set; }
    public UpgradeParkingLot(Wallet wallet)
    {
        _wallet = wallet;
    }
    public void SetParkingLot(ParkingLot parkingLot)
    {
        _currentParkingLot = parkingLot;
        ParkingLotConfiguration = parkingLot.ParkingLotConfiguration;
    }
    private void UpgradeConfirm()
    {
        _currentParkingLot.SetCarConfiguration();
    }
    public bool EngineUpgrade()
    {
        if ((ParkingLotConfiguration.EnginePowerCurrentIndex < ParkingLotConfiguration.MaxIndexEnginePower) == true && (_wallet.TakeCash(ParkingLotConfiguration.GetPriceEngineUpgradeInt()) == true))
        {
            ParkingLotConfiguration.EnginePowerCurrentIndex = ++ParkingLotConfiguration.EnginePowerCurrentIndex;
            UpgradeConfirm();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GearboxUpgrade()
    {
        if ((ParkingLotConfiguration.GearRatioCurrentIndex < ParkingLotConfiguration.MaxIndexGearRatio) == true && (_wallet.TakeCash(ParkingLotConfiguration.GetPriceGearRatioUpgradeInt())) == true)
        {
            ParkingLotConfiguration.GearRatioCurrentIndex = ++ParkingLotConfiguration.GearRatioCurrentIndex;
            UpgradeConfirm();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool WheelUpgrade()
    {
        if ((ParkingLotConfiguration.WheelsCurrentIndex < ParkingLotConfiguration.MaxIndexWheels) == true && (_wallet.TakeCash(ParkingLotConfiguration.GetPriceWheelsUpgradeInt())) == true)
        {
            ParkingLotConfiguration.WheelsCurrentIndex = ++ParkingLotConfiguration.WheelsCurrentIndex;
            UpgradeConfirm();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GunUpgrade()
    {
        if ((ParkingLotConfiguration.GunCurrentIndex < ParkingLotConfiguration.MaxIndexGun) == true && (_wallet.TakeCash(ParkingLotConfiguration.GetPriceGunUpgradeInt())) == true)
        {
            ParkingLotConfiguration.GunCurrentIndex = ++ParkingLotConfiguration.GunCurrentIndex;
            UpgradeConfirm();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CorpusUpgrade()
    {
        if ((ParkingLotConfiguration.CorpusCurrentIndex < ParkingLotConfiguration.MaxIndexCorpus) == true && (_wallet.TakeCash(ParkingLotConfiguration.GetPriceCorpusUpgradeInt())) == true)
        {
            ParkingLotConfiguration.CorpusCurrentIndex = ++ParkingLotConfiguration.CorpusCurrentIndex;
            UpgradeConfirm();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool BoosterUpgrade()
    {
        if ((ParkingLotConfiguration.BoosterCurrentIndex < ParkingLotConfiguration.MaxIndexBooster) == true && (_wallet.TakeCash(ParkingLotConfiguration.GetPriceBoosterUpgradeInt())) == true)
        {
            ParkingLotConfiguration.BoosterCurrentIndex = ++ParkingLotConfiguration.BoosterCurrentIndex;
            UpgradeConfirm();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool FueltankUpgrade()
    {
        if ((ParkingLotConfiguration.FuelQuantityCurrentIndex < ParkingLotConfiguration.MaxIndexFuelQuantity) == true && (_wallet.TakeCash(ParkingLotConfiguration.GetPriceFuelUpgradeInt())) == true)
        {
            ParkingLotConfiguration.FuelQuantityCurrentIndex = ++ParkingLotConfiguration.FuelQuantityCurrentIndex;
            UpgradeConfirm();
            return true;
        }
        else
        {
            return false;
        }
    }
}
