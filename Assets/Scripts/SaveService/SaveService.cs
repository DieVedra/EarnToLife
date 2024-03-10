using UnityEngine;

public class SaveService
{
    private const string FILE_NAME = "/SaveData.dat";
    private readonly string _savePath;
    private ISaveMetod _saveMetod;
    private bool _saveOn;
    public SaveService(bool key)
    {
        //_savePath = Path.Combine(Application.persistentDataPath, FILE_NAME);
        _savePath = Application.dataPath + FILE_NAME;
        _saveMetod = new BinarySave();
        _saveOn = key;
    }
    public PlayerData GetPlayerConfigAfterLoading(int countConfigurations, int testMoney)
    {
        SaveData saveData;
        if (_saveOn == true)
        {
            saveData = (SaveData)_saveMetod.Load(_savePath);
            return CreatePlayerConfig(saveData);
        }
        else
        {
            return CreatePlayerConfigDefault(countConfigurations, testMoney);
        }

        // if (saveData != null)
        // {
        //     return CreatePlayerConfig(saveData);
        // }
        // else
        // {
        //     return CreatePlayerConfigDefault(countConfigurations, testMoney);
        // }
    }
    public void SetPlayerDataToSaving(IPlayerData playerData)
    {
        if (_saveOn == true)
        {
            _saveMetod.Save(_savePath, CreateSaveData(playerData));
        }
    }
    private PlayerData CreatePlayerConfig(SaveData saveData)
    {
        CarConfigurationInParkingLot[] parkingLotIndexes = new CarConfigurationInParkingLot[saveData.SavesParkingsIndexes.Length];
        for (int i = 0; i < parkingLotIndexes.Length; i++)
        {
            parkingLotIndexes[i] = new CarConfigurationInParkingLot(
                saveData.SavesParkingsIndexes[i].EnginePowerCurrentIndex,
                saveData.SavesParkingsIndexes[i].GearRatioCurrentIndex,
                saveData.SavesParkingsIndexes[i].WheelsCurrentIndex,
                saveData.SavesParkingsIndexes[i].GunCurrentIndex,
                saveData.SavesParkingsIndexes[i].CorpusCurrentIndex,
                saveData.SavesParkingsIndexes[i].BoosterCurrentIndex,
                saveData.SavesParkingsIndexes[i].FuelQuantityCurrentIndex
                );
        }

        return new PlayerData(
            new Wallet(saveData.Money),
            new GarageConfig(parkingLotIndexes, saveData.CurrentSelectLotCarIndex, saveData.AvailableLotCarIndex),
            saveData.Level,
            saveData.Days,
            saveData.NewLevelHasBeenOpen,
            saveData.SoundOn,
            saveData.MusicOn
            );

    }
    private PlayerData CreatePlayerConfigDefault(int countConfigurations, int testMoney)
    {
        CarConfigurationInParkingLot[] parkingLotIndexes = new CarConfigurationInParkingLot[countConfigurations];
        for (int i = 0; i < parkingLotIndexes.Length; i++)
        {
            parkingLotIndexes[i] = new CarConfigurationInParkingLot();
        }
        return new PlayerData(new Wallet(testMoney), new GarageConfig(parkingLotIndexes));
    }
    private SaveData CreateSaveData(IPlayerData playerData)
    {
        SaveParkingIndexes[] savesParkingsIndexes = new SaveParkingIndexes[
            playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes.Count];
        for (int i = 0; i < savesParkingsIndexes.Length; i++)
        {
            savesParkingsIndexes[i] = new SaveParkingIndexes
            {
                EnginePowerCurrentIndex = playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[i].EnginePowerIndex,
                GearRatioCurrentIndex = playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[i].GearRatioIndex,
                WheelsCurrentIndex = playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[i].WheelsIndex,
                GunCurrentIndex = playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[i].GunIndex,
                CorpusCurrentIndex = playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[i].CorpusIndex,
                BoosterCurrentIndex = playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[i].BoosterIndex,
                FuelQuantityCurrentIndex = playerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[i].FuelQuantityIndex
            };
        }
        return new SaveData()
        {
            CurrentSelectLotCarIndex = playerData.GarageConfig.CurrentSelectLotCarIndex,
            AvailableLotCarIndex = playerData.GarageConfig.AvailableLotCarIndex,
            Money = playerData.Wallet.Money,
            Days = playerData.Days,
            Level = playerData.Level,
            NewLevelHasBeenOpen = playerData.NewLevelHasBeenOpen,
            SoundOn = playerData.SoundOn,
            MusicOn = playerData.MusicOn,
            SavesParkingsIndexes = savesParkingsIndexes
        };
    }
}
