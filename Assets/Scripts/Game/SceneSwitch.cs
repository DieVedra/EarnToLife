using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class SceneSwitch
{
    private GameData _gameData;
    private readonly IconLoadHandler _iconLoadHandler;
    private PlayerDataHandler _playerDataHandler;
    private AsyncOperation _asyncOperation;
    public float LoadProgress => _asyncOperation.progress;
    public SceneSwitch(PlayerDataHandler playerDataHandler, GameData gameData, IconLoadHandler iconLoadHandler)
    {
        _playerDataHandler = playerDataHandler;
        _gameData = gameData;
        _iconLoadHandler = iconLoadHandler;
    }
    private void StartLoadSceneAsync(int index)
    {
        if (_asyncOperation == null)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(index);
            _asyncOperation.allowSceneActivation = false;
            _asyncOperation.completed += OnCompleteOperation;
        }
    }
    public void StartLoadLastLevel()
    {
        
        StartLoadSceneAsync(_playerDataHandler.PlayerData.Level);
        // StartLoadSceneAsync(1);



        //Debug.Log("Level start load: " + _gameData.PlayerConfig.Level);
    }
    public void StartLoadGarage()
    {
        StartLoadSceneAsync(0);
        _gameData.StartPanelInMenu = PanelsInMenu.GaragePanel;
    }
    public void StartLoadMap()
    {
        StartLoadSceneAsync(0);
        _gameData.StartPanelInMenu = PanelsInMenu.MapPanel;
    }
    public void EndLoadingSceneAsync()
    {
        if (_asyncOperation != null)
        {
            _asyncOperation.allowSceneActivation = true;
        }
    }

    private void OnCompleteOperation(AsyncOperation asyncOperation)
    {
        _asyncOperation.completed -= OnCompleteOperation;
        _iconLoadHandler.Dispose();
    }
}
