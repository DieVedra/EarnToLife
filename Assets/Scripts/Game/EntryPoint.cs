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
    [Inject] private GameData _gameData;
    [Inject] private SaveService _saveService;
    [Inject] private PlayerDataProvider _playerDataProvider;
    [Inject] private GlobalAudio _globalAudio;
    [Inject] private Garage _garage;
    [Inject] private GarageData _garageData;
    [Inject] private SpriteRenderer _startMenuBackground;
    [Inject] private Map _map;
    [Inject] private ViewEntryScene _viewEntryScene;
    [Inject] private AudioClipProvider _audioClipProvider;

    private PresenterEntryScene _presenterEntryScene;
    private IPlayerData _playerData;
    private PlayerDataHandler _playerDataHandler;
    private void Awake()
    {
        _gameData.StartPanelInMenu = _startPanelInMenu;
        _gameData.CarControlMethod = carControlMethod;
        
        PlayerDataInit();
        _globalAudio.Construct(_audioClipProvider, _playerData.SoundOn, _playerData.MusicOn);
        _garage.Construct(_playerData, _garageData);
        _presenterEntryScene = new PresenterEntryScene(_viewEntryScene, _garage, _map, _startMenuBackground, _gameData, _garageData, _globalAudio, _playerDataHandler);
    }

    private void PlayerDataInit()
    {
        _playerDataProvider.PlayerDataHandler ??= new PlayerDataHandler(
            _saveService.GetPlayerConfigAfterLoading(_garageData.ParkingLotsConfigurations.Count, _cash));
        _playerDataHandler = _playerDataProvider.PlayerDataHandler;
        _playerData = _playerDataHandler.PlayerData;
    }

    private void OnApplicationQuit()
    {
        _saveService.SetPlayerDataToSaving(_playerData);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause is true)
        {
            _saveService.SetPlayerDataToSaving(_playerData);
        }
    }
    private void OnDestroy()
    {
        // Debug.Log("Main Destr");
    }
}
