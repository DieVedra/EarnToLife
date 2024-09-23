using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogicUILevel
{
    private readonly PausePanelButtonsHandler _pausePanelButtonsHandler;
    private readonly PanelsHandler _panelsHandler;
    private readonly DevicePanelHandler _devicePanelHandler;
    private readonly ResultsLevelHandler _resultsLevelHandler;
    private readonly LevelProgressHandler _levelProgressHandler;
    private readonly NotificationsHandler _notificationsHandler;
    private readonly AudioSettingSwitch _audioSettingSwitch;
    private readonly IconLoadHandler _iconLoadHandler;
    private readonly SceneSwitch _sceneSwitch;
    private CarBoosterHandler _carBoosterHandler;
    private CarGunHandler _carGunHandler;
    private ButtonsControl _buttonsControl;

    public LogicUILevel(ViewUILevel viewUILevel, CarInLevel carInLevel, GamePause gamePause,
        ResultsLevelProvider resultsLevelProvider, PlayerDataHandler playerDataHandler, GlobalAudio globalAudio,
        TextMeshProUGUI notificationPrefab, GameData gameData)
    {
        TryInitButtonsControl(viewUILevel, carInLevel, gameData.GetCarControlMethod());
        _iconLoadHandler = new IconLoadHandler(viewUILevel.IconLoad);
        _sceneSwitch = new SceneSwitch(playerDataHandler, gameData, _iconLoadHandler);
        _pausePanelButtonsHandler = new PausePanelButtonsHandler(viewUILevel.PanelPause);
        _resultsLevelHandler = new ResultsLevelHandler(viewUILevel.PanelScore, viewUILevel.DisposeCommand);
        _audioSettingSwitch = new AudioSettingSwitch(globalAudio,
            viewUILevel.PanelPause.TextOnOffButtonMusic,
            viewUILevel.PanelPause.TextOnOffButtonSound);
        _panelsHandler = new PanelsHandler(viewUILevel, _sceneSwitch, _pausePanelButtonsHandler,
            _buttonsControl, gamePause, resultsLevelProvider, _resultsLevelHandler, _audioSettingSwitch, _iconLoadHandler,
            new AudioHandlerUI(globalAudio), viewUILevel.DisposeCommand);
        _notificationsHandler = new NotificationsHandler(viewUILevel, resultsLevelProvider, notificationPrefab, viewUILevel.DisposeCommand);
        _levelProgressHandler = new LevelProgressHandler(viewUILevel, resultsLevelProvider.LevelProgressCounter, viewUILevel.DisposeCommand);

        TryInitGun(viewUILevel, carInLevel);
        TryInitBooster(viewUILevel, carInLevel);
        _devicePanelHandler = new DevicePanelHandler(viewUILevel.DevicePanel, carInLevel, _carBoosterHandler, viewUILevel.DisposeCommand);
    }
    private void TryInitGun(ViewUILevel viewUILevel, CarInLevel carInLevel)
    {
        if (carInLevel.GunAvailable)
        {
            _carGunHandler = new CarGunHandler(viewUILevel, carInLevel.CarGun);
        }
        else
        {
            viewUILevel.ButtonCrosshair.gameObject.SetActive(false);
            viewUILevel.ButtonFire.interactable = false;
            viewUILevel.ButtonFire.gameObject.SetActive(false);
        }
    }

    private void TryInitBooster(ViewUILevel viewUILevel, CarInLevel carInLevel)
    {
        if (carInLevel.BoosterAvailable)
        {
            _carBoosterHandler = new CarBoosterHandler(viewUILevel, carInLevel);
        }
        else
        {
            viewUILevel.ButtonBoost.interactable = false;
            viewUILevel.ButtonBoost.gameObject.SetActive(false);
        }
    }

    private void TryInitButtonsControl(ViewUILevel viewUILevel, CarInLevel carInLevel, CarControlMethod carControlMethod)
    {
        if (carControlMethod == CarControlMethod.SensorDisplayMethod)
        {
            _buttonsControl = new ButtonsControl(viewUILevel, carInLevel);
        }
        else
        {
            _buttonsControl = null;
            IReadOnlyList<Button> _buttons = viewUILevel.GetUIControlButtons();
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
