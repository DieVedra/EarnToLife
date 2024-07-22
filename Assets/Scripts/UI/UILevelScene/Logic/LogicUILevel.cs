using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogicUILevel
{
    private ButtonsControl _buttonsControl;
    private PausePanelButtonsHandler _pausePanelButtonsHandler;
    private PanelsHandler _panelsHandler;
    private DevicePanelHandler _devicePanelHandler;
    private ResultsLevelHandler _resultsLevelHandler;
    private LevelProgressHandler _levelProgressHandler;
    private NotificationsHandler _notificationsHandler;
    private CarGunHandler _carGunHandler;
    private CarBoosterHandler _carBoosterHandler;
    private AudioSettingSwitch _audioSettingSwitch;
    public LogicUILevel(ViewUILevel viewUILevel, CarInLevel carInLevel, GamePause gamePause,
        ResultsLevelProvider resultsLevelProvider, SceneSwitch sceneSwitch, GlobalAudio globalAudio,
        TextMeshProUGUI notificationPrefab, CarControlMethod carControlMethod)
    {
        TryInitButtonsControl(viewUILevel, carInLevel, carControlMethod);
        _pausePanelButtonsHandler = new PausePanelButtonsHandler(viewUILevel.PanelPause);
        _resultsLevelHandler = new ResultsLevelHandler(viewUILevel.PanelScore, viewUILevel.DisposeCommand);
        _audioSettingSwitch = new AudioSettingSwitch(globalAudio,
            viewUILevel.PanelPause.TextOnOffButtonMusic,
            viewUILevel.PanelPause.TextOnOffButtonSound);
        _panelsHandler = new PanelsHandler(viewUILevel, sceneSwitch, _pausePanelButtonsHandler,
            _buttonsControl, gamePause, resultsLevelProvider, _resultsLevelHandler, _audioSettingSwitch,
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
        }
    }

    private void TryInitButtonsControl(ViewUILevel viewUILevel, CarInLevel carInLevel, CarControlMethod carControlMethod)
    {
        if (carControlMethod == CarControlMethod.UIMethod)
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
