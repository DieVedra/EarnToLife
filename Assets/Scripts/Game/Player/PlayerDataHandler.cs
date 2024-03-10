using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandler
{
    private PlayerData _playerData;
    public IPlayerData PlayerData => _playerData;
    public PlayerDataHandler(PlayerData playerData)
    {
        _playerData = playerData;
    }
    public ResultLevelForSlider TryGetResultLevelForSlider()
    {
        if(_playerData.NewLevelHasBeenOpen == true)
        {
            return null;
        }
        else
        {
            return _playerData.ResultsLevel?.ResultLevelForSlider;
        }
    }

    public CarConfigurationInParkingLot GetCurrentCarConfigurationInParkingLot()
    {
        return PlayerData.GarageConfig.CarConfigurationsInParkingLotsIndexes[PlayerData.GarageConfig.CurrentSelectLotCarIndex];
    }

    public int GetCurrentSelectLotCarIndex()
    {
        return PlayerData.GarageConfig.CurrentSelectLotCarIndex;
    }
    public ResultsLevel TryGetResultLevel()
    {
        if (_playerData.NewLevelHasBeenOpen == true)
        {
            //NewLevelHasBeenOpen = false;

            return null;

        }
        else
        {
            return _playerData.ResultsLevel;
        }
    }
    public void SetResultsLevel(ResultsLevel resultsLevel)
    {
        _playerData.ResultsLevel = resultsLevel;
    }
    public void OpenNextLevel()
    {
        _playerData.Level++;
        _playerData.NewLevelHasBeenOpen = true;
    }
    public void AddDay()
    {
        _playerData.Days++;
    }
    public void LevelWasNotPassed()
    {
        _playerData.NewLevelHasBeenOpen = false;
    }
}