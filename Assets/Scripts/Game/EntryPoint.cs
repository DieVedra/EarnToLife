using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using NaughtyAttributes;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PanelsInMenu _startPanelInMenu;
    [SerializeField, HorizontalLine(color: EColor.Gray), BoxGroup("Cash Wallet")] private int _cash;
    [SerializeField] private CarControlMethod carControlMethod;
    [SerializeField] private bool _saveOn;
    [Inject] private GameData _gameData;
    [Inject] private SaveService _saveService;
    [Inject] private PlayerDataProvider _playerDataProvider;
    [Inject] private GlobalAudio _globalAudio;
    [Inject] private Garage _garage;
    [Inject] private GarageData _garageData;
    [Inject] private SpriteRenderer _startMenuBackground;
    [Inject] private Map _map;
    [Inject] private ViewEntryScene _viewEntryScene;

    private PresenterEntryScene _presenterEntryScene;
    private IPlayerData _playerData;
    private PlayerDataHandler _playerDataHandler;
    private void Awake()
    {
        Time.timeScale = 1f;
        PlayerDataInit();
        GameDataInit();
        _globalAudio.InitFromEntryScene(_playerDataProvider.PlayerDataHandler);
        _garage.Construct(_playerData, _garageData);
        _presenterEntryScene = new PresenterEntryScene(_viewEntryScene, _garage, _map, _startMenuBackground, _gameData, _garageData, _globalAudio, _playerDataHandler);
    }

    private void GameDataInit()
    {
        _gameData.StartPanelInMenu = _startPanelInMenu;
        _gameData.CarControlMethod = carControlMethod;
        _gameData.SaveOn = _saveOn;
    }
    private void PlayerDataInit()
    {
        _playerDataProvider.PlayerDataHandler ??= new PlayerDataHandler(
            _saveService.GetPlayerConfigAfterLoading(_garageData.ParkingLotsConfigurations.Count, _cash, _saveOn));
        _playerDataHandler = _playerDataProvider.PlayerDataHandler;
        _playerData = _playerDataHandler.PlayerData;
    }

    private void OnApplicationQuit()
    {
        // Debug.Log($"OnApplicationQuit Entry  SaveOn: {_gameData.SaveOn}");
        _saveService.SetPlayerDataToSaving(_playerData, _saveOn);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause is true)
        {
            _saveService.SetPlayerDataToSaving(_playerData, _saveOn);
        }
    }
    private void OnDestroy()
    {
        // Debug.Log($"OnDestroy");
    }
}
