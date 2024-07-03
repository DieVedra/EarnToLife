using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "ParkingLot", menuName = "GarageContent/ParkingLot", order = 51)]
public class ParkingLotConfiguration : ScriptableObject
{
    private const string MAX = "Max";

    [SerializeField, HorizontalLine(color: EColor.Gray), BoxGroup("Defaults Values")] private float _defaultEngineOverclockingMultiplier;
    [SerializeField, BoxGroup("Defaults Values")] private float _defaultGearRatio;
    [SerializeField, BoxGroup("Defaults Values")] private float _defaultFuelQuantityValue;
    private int _defaultWheelsValue = 0;
    private int _defaultGunValue = 0;
    private int _defaultCorpusValue = 0;
    private float _defaultBoosterValue = 0f;


    [SerializeField, HorizontalLine(color: EColor.Yellow), BoxGroup("PriceTagForTheMurderThisCar")] private float _priceTagMurder;
    [SerializeField, HorizontalLine(color: EColor.Green), BoxGroup("PriceTagForTheWayMeterThisCar")] private float _priceTagWayMeter;
    [SerializeField, HorizontalLine(color: EColor.Green), BoxGroup("PriceTagForTheDestructionBoxOrBeamThisCar")] private float _priceTagDestruction;

    [SerializeField, HorizontalLine(color: EColor.Blue), BoxGroup("Upgrades Values and Prices Values")] private UpgradeValueFloat[] _engineOverclockingMultiplierUpgrade;
    [SerializeField, BoxGroup("Upgrades Values and Prices Values")] private UpgradeValueFloat[] _gearRatioUpgrade;
    [SerializeField, BoxGroup("Upgrades Values and Prices Values")] private UpgradeValueInt[] _wheelsUpgrade;
    [SerializeField, BoxGroup("Upgrades Values and Prices Values")] private UpgradeValueInt[] _gunUpgrade;
    [SerializeField, BoxGroup("Upgrades Values and Prices Values")] private UpgradeValueInt[] _corpusUpgrade;
    [SerializeField, BoxGroup("Upgrades Values and Prices Values")] private UpgradeValueFloat[] _boosterUpgrade;
    [SerializeField, BoxGroup("Upgrades Values and Prices Values")] private UpgradeValueFloat[] _fuelQuantityUpgrade;

    private float[] _engineOverclockingMultiplierUpgradeValuesWithDefault;
    private float[] _gearRatioUpgradeValuesWithDefault;
    private int[] _wheelsUpgradeValuesWithDefault;
    private int[] _gunUpgradeValuesWithDefault;
    private int[] _corpusUpgradeValuesWithDefault;
    private float[] _boosterUpgradeValuesWithDefault;
    private float[] _fuelQuantityUpgradeValuesWithDefault;

    private string[] _pricesEngine;
    private string[] _pricesGearRatio;
    private string[] _pricesWheels;
    private string[] _pricesGun;
    private string[] _pricesCorpus;
    private string[] _pricesBooster;
    private string[] _pricesFuelQuantity;
    private CarConfigurationInParkingLot _carConfigurationInParkingLot;
    #region GetPriceUpgradeInt
    public int GetPriceEngineUpgradeInt()
    {
        return _engineOverclockingMultiplierUpgrade[_carConfigurationInParkingLot.EnginePowerIndex].Price;
    }
    public int GetPriceGearRatioUpgradeInt()
    {
        return _gearRatioUpgrade[_carConfigurationInParkingLot.GearRatioIndex].Price;
    }
    public int GetPriceWheelsUpgradeInt()
    {
        return _wheelsUpgrade[_carConfigurationInParkingLot.WheelsIndex].Price;
    }
    public int GetPriceGunUpgradeInt()
    {
        return _gunUpgrade[_carConfigurationInParkingLot.GunIndex].Price;
    }
    public int GetPriceCorpusUpgradeInt()
    {
        return _corpusUpgrade[_carConfigurationInParkingLot.CorpusIndex].Price;
    }
    public int GetPriceBoosterUpgradeInt()
    {
        return _boosterUpgrade[_carConfigurationInParkingLot.BoosterIndex].Price;
    }
    public int GetPriceFuelUpgradeInt()
    {
        return _fuelQuantityUpgrade[_carConfigurationInParkingLot.FuelQuantityIndex].Price;
    }
    #endregion

    #region GetMaxIndex

    public int MaxIndexEnginePower => _engineOverclockingMultiplierUpgrade.Length;

    public int MaxIndexGearRatio => _gearRatioUpgrade.Length;

    public int MaxIndexWheels => _wheelsUpgrade.Length;

    public int MaxIndexGun => _gunUpgrade.Length;

    public int MaxIndexCorpus => _corpusUpgrade.Length;

    public int MaxIndexBooster => _boosterUpgrade.Length;

    public int MaxIndexFuelQuantity => _fuelQuantityUpgrade.Length;

    #endregion

    #region GetCurrentIndex

    public int EnginePowerCurrentIndex
    {
        get => _carConfigurationInParkingLot.EnginePowerIndex;
        set
        {
            if (value == _carConfigurationInParkingLot.EnginePowerIndex + 1 && value <= MaxIndexEnginePower)
            {
                _carConfigurationInParkingLot.SetEnginePowerIndex(value);
            }
        }
    }

    public int GearRatioCurrentIndex
    {
        get => _carConfigurationInParkingLot.GearRatioIndex;
        set
        {
            if (value == _carConfigurationInParkingLot.GearRatioIndex + 1 && value <= MaxIndexGearRatio)
            {
                _carConfigurationInParkingLot.SetGearRatioIndex(value);
            }
        }
    }

    public int WheelsCurrentIndex
    {
        get => _carConfigurationInParkingLot.WheelsIndex;
        set
        {
            if (value == _carConfigurationInParkingLot.WheelsIndex + 1 && value <= MaxIndexWheels)
            {
                _carConfigurationInParkingLot.SetWheelsIndex(value);
            }
        }
    }

    public int GunCurrentIndex
    {
        get => _carConfigurationInParkingLot.GunIndex;
        set
        {
            if (value == _carConfigurationInParkingLot.GunIndex + 1 && value <= MaxIndexGun)
            {
                _carConfigurationInParkingLot.SetGunIndex(value);
            }
        }
    }

    public int CorpusCurrentIndex
    {
        get => _carConfigurationInParkingLot.CorpusIndex;
        set
        {
            if (value == _carConfigurationInParkingLot.CorpusIndex + 1 && value <= MaxIndexCorpus)
            {
                _carConfigurationInParkingLot.SetCorpusIndex(value);
            }
        }
    }

    public int BoosterCurrentIndex
    {
        get => _carConfigurationInParkingLot.BoosterIndex;
        set
        {
            if (value == _carConfigurationInParkingLot.BoosterIndex + 1 && value <= MaxIndexBooster)
            {
                _carConfigurationInParkingLot.SetBoosterIndex(value);
            }
        }
    }

    public int FuelQuantityCurrentIndex
    {
        get => _carConfigurationInParkingLot.FuelQuantityIndex;
        set
        {
            if (value == _carConfigurationInParkingLot.FuelQuantityIndex + 1 && value <= MaxIndexFuelQuantity)
            {
                _carConfigurationInParkingLot.SetFuelQuantityIndex(value);
            }
        }
    }

    #endregion

    #region GetUpgradeValue

    public float GetEnginePowerValue()
    {
        return _engineOverclockingMultiplierUpgradeValuesWithDefault[EnginePowerCurrentIndex];
    }

    public float GetGearRatioValue()
    {
        return _gearRatioUpgradeValuesWithDefault[GearRatioCurrentIndex];
    }

    public int GetWheelsValue()
    {
        return _wheelsUpgradeValuesWithDefault[WheelsCurrentIndex];
    }

    public int GetGunAmmoValue()
    {
        return _gunUpgradeValuesWithDefault[GunCurrentIndex];
    }

    public int GetCorpusUpgradeValue()
    {
        return _corpusUpgradeValuesWithDefault[CorpusCurrentIndex];
    }

    public float GetBoosterFuelValue()
    {
        return _boosterUpgradeValuesWithDefault[BoosterCurrentIndex];
    }

    public float GetFuelQuantityValue()
    {
        return _fuelQuantityUpgradeValuesWithDefault[FuelQuantityCurrentIndex];
    }

    #endregion

    public IReadOnlyList<int> GetCurrentIndexesValues
    {
        get
        {
            return new int[]
            {
                _carConfigurationInParkingLot.EnginePowerIndex,
                _carConfigurationInParkingLot.GearRatioIndex,
                _carConfigurationInParkingLot.WheelsIndex,
                _carConfigurationInParkingLot.GunIndex,
                _carConfigurationInParkingLot.CorpusIndex,
                _carConfigurationInParkingLot.BoosterIndex,
                _carConfigurationInParkingLot.FuelQuantityIndex
            };
        }
    }

    public IReadOnlyList<int> PricesUpgrade
    {
        get
        {
            return new int[]
            {
                GetPriceEngineUpgradeInt(),
                GetPriceGearRatioUpgradeInt(),
                GetPriceWheelsUpgradeInt(),
                GetPriceGunUpgradeInt(),
                GetPriceCorpusUpgradeInt(),
                GetPriceBoosterUpgradeInt(),
                GetPriceFuelUpgradeInt()
            };
        }
    }

    public IReadOnlyList<int> GetPricesUpgradeForColors
    {
        get
        {
            return new int[]
            {
                Parser(GetPriceEngineUpgradeString()),
                Parser(GetPriceGearRatioUpgradeString()),
                Parser(GetPriceWheelsUpgradeString()),
                Parser(GetPriceGunUpgradeString()),
                Parser(GetPriceCorpusUpgradeString()),
                Parser(GetPriceBoosterUpgradeString()),
                Parser(GetPriceFuelUpgradeString())
            };
        }
    }

    public IReadOnlyList<string> GetPricesString
    {
        get
        {
            return new string[]
            {
                GetPriceEngineUpgradeString(),
                GetPriceGearRatioUpgradeString(),
                GetPriceWheelsUpgradeString(),
                GetPriceGunUpgradeString(),
                GetPriceCorpusUpgradeString(),
                GetPriceBoosterUpgradeString(),
                GetPriceFuelUpgradeString()
            };
        }
    }

    public IReadOnlyList<int> GetMaxIndexesValues
    {
        get
        {
            return new int[]
            {
                MaxIndexEnginePower,
                MaxIndexGearRatio,
                MaxIndexWheels,
                MaxIndexGun,
                MaxIndexCorpus,
                MaxIndexBooster,
                MaxIndexFuelQuantity
            };
        }
    }

    public float GetMaxFuelQuantity()
    {
        return _fuelQuantityUpgrade[_fuelQuantityUpgrade.Length - 1].Value;
    }
    public float GetMaxBoosterFuelQuantity()
    {
        return _boosterUpgrade[_boosterUpgrade.Length - 1].Value;
    }
    public float PriceTagForTheMurder => _priceTagMurder;

    public float PriceTagForTheWayMeter => _priceTagWayMeter;
    public float PriceTagForTheDestruction => _priceTagDestruction;

    public void Construct(CarConfigurationInParkingLot carConfigurationInParkingLot)
    {
        _engineOverclockingMultiplierUpgradeValuesWithDefault = AddDefaultValueFloat(_engineOverclockingMultiplierUpgrade, _defaultEngineOverclockingMultiplier);
        _gearRatioUpgradeValuesWithDefault = AddDefaultValueFloat(_gearRatioUpgrade, _defaultGearRatio);
        _wheelsUpgradeValuesWithDefault = AddDefaultValueInt(_wheelsUpgrade, _defaultWheelsValue);
        _gunUpgradeValuesWithDefault = AddDefaultValueInt(_gunUpgrade, _defaultGunValue);
        _corpusUpgradeValuesWithDefault = AddDefaultValueInt(_corpusUpgrade, _defaultCorpusValue);
        _boosterUpgradeValuesWithDefault = AddDefaultValueFloat(_boosterUpgrade, _defaultBoosterValue);
        _fuelQuantityUpgradeValuesWithDefault = AddDefaultValueFloat(_fuelQuantityUpgrade, _defaultFuelQuantityValue);

        _pricesEngine = CreatePricesStrings(_engineOverclockingMultiplierUpgrade);
        _pricesGearRatio = CreatePricesStrings(_gearRatioUpgrade);
        _pricesWheels = CreatePricesStrings(_wheelsUpgrade);
        _pricesGun = CreatePricesStrings(_gunUpgrade);
        _pricesCorpus = CreatePricesStrings(_corpusUpgrade);
        _pricesBooster = CreatePricesStrings(_boosterUpgrade);
        _pricesFuelQuantity = CreatePricesStrings(_fuelQuantityUpgrade);

        _carConfigurationInParkingLot = carConfigurationInParkingLot;
    }

    #region GetPriceUpgradeString

    private string GetPriceEngineUpgradeString()
    {
        return _pricesEngine[_carConfigurationInParkingLot.EnginePowerIndex];
    }

    private string GetPriceGearRatioUpgradeString()
    {
        return _pricesGearRatio[_carConfigurationInParkingLot.GearRatioIndex];
    }

    private string GetPriceWheelsUpgradeString()
    {
        return _pricesWheels[_carConfigurationInParkingLot.WheelsIndex];
    }

    private string GetPriceGunUpgradeString()
    {
        return _pricesGun[_carConfigurationInParkingLot.GunIndex];
    }

    private string GetPriceCorpusUpgradeString()
    {
        return _pricesCorpus[_carConfigurationInParkingLot.CorpusIndex];
    }

    private string GetPriceBoosterUpgradeString()
    {
        return _pricesBooster[_carConfigurationInParkingLot.BoosterIndex];
    }

    private string GetPriceFuelUpgradeString()
    {
        return _pricesFuelQuantity[_carConfigurationInParkingLot.FuelQuantityIndex];
    }
    #endregion

    private int Parser(string value)
    {
        if (value != MAX)
        {
            return int.Parse(value);
        }
        else
        {
            return 0;
        }
    }

    private int[] AddDefaultValueInt(UpgradeValueInt[] array, int value)
    {
        int[] numbers = new int[array.Length + 1];
        numbers[0] = value;
        for (int i = 1; i < numbers.Length; i++)
        {
            numbers[i] = array[i-1].Value;
        }
        return numbers;
    }
    private float[] AddDefaultValueFloat(UpgradeValueFloat[] array, float value)
    {
        float[] numbers = new float[array.Length + 1];
        numbers[0] = value;
        for (int i = 1; i < numbers.Length; i++)
        {
            numbers[i] = array[i-1].Value;
        }
        return numbers;
    }
    private string[] CreatePricesStrings(Upgrade[] array)
    {
        string[] strings = new string[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
        {
            strings[i] = array[i].Price.ToString();
        }
        strings[array.Length] = MAX;
        return  strings;
    }
    public void ResetIndexes()
    {
        _carConfigurationInParkingLot.SetEnginePowerIndex(0);
        _carConfigurationInParkingLot.SetGearRatioIndex(0);
        _carConfigurationInParkingLot.SetWheelsIndex(0);
        _carConfigurationInParkingLot.SetGunIndex(0);
        _carConfigurationInParkingLot.SetCorpusIndex(0);
        _carConfigurationInParkingLot.SetBoosterIndex(0);
        _carConfigurationInParkingLot.SetFuelQuantityIndex(0);
    }

    [Serializable]
    private class Upgrade 
    {
        public int Price;
    }
    [Serializable]
    private class UpgradeValueInt : Upgrade
    {
        public int Value;
    }
    [Serializable]
    private class UpgradeValueFloat : Upgrade
    {
        public float Value;
    }
}