using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;
using NaughtyAttributes;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [Inject, SerializeField, Expandable] private GameData _gameData;
    [Inject] private SaveService _saveService;
    [Inject] private PlayerDataProvider _playerDataProvider;
    [Inject] private GlobalAudio _globalAudio;
    [Inject] private Garage _garage;
    [Inject] private GarageData _garageData;
    [Inject] private SpriteRenderer _startMenuBackground;
    [Inject] private Map _map;
    [Inject] private ViewEntryScene _viewEntryScene;

    private LogicEntryScene _logicEntryScene;
    private IPlayerData _playerData;
    private PlayerDataHandler _playerDataHandler;
    private void Awake()
    {
        Time.timeScale = 1f;
        PlayerDataInit();
        _globalAudio.InitFromEntryScene(_playerDataProvider.PlayerDataHandler);
        _garage.Init(_playerData, _garageData);
        _logicEntryScene = new LogicEntryScene(_garage, _map, _startMenuBackground, _viewEntryScene, _gameData, _garageData, _globalAudio, _playerDataHandler);
        _map.Init(_playerData.Level);
        Debug.Log($"Current Level: {_playerData.Level}");
    }
    private void PlayerDataInit()
    {
        _playerDataProvider.PlayerDataHandler ??= new PlayerDataHandler(
            _saveService.GetPlayerConfigAfterLoading(_garageData.ParkingLotsConfigurations.Count, _gameData.Cash, _gameData.SaveOn));
        _playerDataHandler = _playerDataProvider.PlayerDataHandler;
        _playerData = _playerDataHandler.PlayerData;
    }

    private void OnApplicationQuit()
    {
        _saveService.SetPlayerDataToSaving(_playerData, _gameData.SaveOn);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause is true)
        {
            _saveService.SetPlayerDataToSaving(_playerData, _gameData.SaveOn);
        }
    }
}
