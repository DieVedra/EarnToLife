using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PanelsActivator
{
    private ValuesPanelActivator _valuesPanelActivator;
    private StartMenuPanel _startMenuPanel;
    private SettingsPanel _settingsPanel;
    private MapPanel _mapPanel;
    private GaragePanel _garagePanel;
    private Image _background;
    private SpriteRenderer _startMenuBackground;
    private GarageUI _garageUI;
    private SceneSwitch _sceneSwitch;
    private GameData _gameData;
    private MapPanelHandler _mapPanelHandler;
    private RectTransform _rectTransformPanelSettings;
    private AudioHandlerUI _audioHandlerUI;
    private AudioSettingSwitch _audioSettingSwitch;

    private CancellationTokenSource _cancellationTokenSource;

    public PanelsActivator(ViewEntryScene viewEntryScene, GarageUI buttonsSelectLotCar, SceneSwitch sceneSwitch, Image background, SpriteRenderer startMenuBackground, GameData gameData, MapPanelHandler mapPanelHandler, AudioHandlerUI audioHandlerUI, AudioSettingSwitch audioSettingSwitch)
    {
        _valuesPanelActivator = new ValuesPanelActivator();
        _audioSettingSwitch = audioSettingSwitch;
        _audioHandlerUI = audioHandlerUI;
        _startMenuPanel = viewEntryScene.PanelStartMenu;
        _settingsPanel = viewEntryScene.PanelStartMenu.PanelSettings;
        _mapPanel = viewEntryScene.PanelMap;
        _garagePanel = viewEntryScene.PanelGarage;
        _sceneSwitch = sceneSwitch;
        _background = background;
        _garageUI = buttonsSelectLotCar;
        _gameData = gameData;
        _mapPanelHandler = mapPanelHandler;
        _startMenuBackground = startMenuBackground;
        StartGame();
    }

    private void StartGame()
    {
        _startMenuPanel.PanelSettings.gameObject.SetActive(false);
        _rectTransformPanelSettings = _settingsPanel.GetComponent<RectTransform>();
        switch (_gameData.StartPanelInMenu)
        {
            case PanelsInMenu.StartPanel:
                _startMenuPanel.gameObject.SetActive(true);
                _mapPanel.gameObject.SetActive(false);
                _garagePanel.gameObject.SetActive(false);
                _garageUI.Deactivate();
                ActivateStartMenuPanel().Forget();
                break;

            case PanelsInMenu.MapPanel:
                _startMenuPanel.gameObject.SetActive(false);
                _mapPanel.gameObject.SetActive(true);
                _garagePanel.gameObject.SetActive(false);
                _garageUI.Deactivate();
                ActivateMapPanel().Forget();
                break;
            
            case PanelsInMenu.GaragePanel:
                _startMenuPanel.gameObject.SetActive(false);
                _mapPanel.gameObject.SetActive(false);
                _garagePanel.gameObject.SetActive(true);
                ActivateGaragePanel().Forget();
                break;
        }
    }
    
    private async UniTask ActivateStartMenuPanel()
    {
        _startMenuPanel.gameObject.SetActive(true);
        _startMenuBackground.gameObject.SetActive(true);
        _startMenuPanel.ButtonPlay.onClick.AddListener(() =>
        {
            _audioHandlerUI.PlayClick();
            SwitchStartMenuToMap();
        });
        _startMenuPanel.ButtonSettings.onClick.AddListener(()=>
        {
            _audioHandlerUI.PlayClick();
            ActivateSettingsPanel();
        });
        await Activate();
    }
    private async UniTask DeactivateStartMenuPanel()
    {
        _startMenuPanel.ButtonPlay.onClick.RemoveAllListeners();
        _startMenuPanel.ButtonSettings.onClick.RemoveAllListeners();
        await Deactivate();
        _startMenuPanel.gameObject.SetActive(false);
        _startMenuBackground.gameObject.SetActive(false);
    }
    private async UniTask ActivateMapPanel()
    {
        _mapPanel.gameObject.SetActive(true);
        _mapPanelHandler.MapEnable();
        _mapPanel.ButtonBackToStartMenu.onClick.AddListener(()=>
        {
            _audioHandlerUI.PlayClick();
            SwitchMapToStartMenu();
        });
        _mapPanel.ButtonGarage.onClick.AddListener(()=>
        {
            _audioHandlerUI.PlayClick();
            SwitchMapToGarage();
        });
        await Activate();
    }

    private async UniTask DeactivateMapPanel()
    {
        _mapPanel.ButtonBackToStartMenu.onClick.RemoveAllListeners();
        _mapPanel.ButtonGarage.onClick.RemoveAllListeners();
        await Deactivate();
        _mapPanel.gameObject.SetActive(false);
        _mapPanelHandler.MapDisable();
    }
    private async UniTask ActivateGaragePanel()
    {
        _garagePanel.gameObject.SetActive(true);
        _garagePanel.ButtonBackToMap.onClick.AddListener(()=>
        {
            _audioHandlerUI.PlayClick();
            SwitchGarageToMap();
        });
        _garagePanel.ButtonGO.onClick.AddListener((() =>
        {
            _audioHandlerUI.PlayClick();
            LoadGame();
        }));
        _garageUI.Activate();
        await Activate();
    }

    private async UniTask DeactivateGaragePanel()
    {
        _garagePanel.ButtonBackToMap.onClick.RemoveAllListeners();
        _garagePanel.ButtonGO.onClick.RemoveAllListeners();
        await MoveAndFadeWhenAll(_garageUI.DeactivateUpgradeButtons(), Deactivate());
        _garageUI.Deactivate();
        _garagePanel.gameObject.SetActive(false);
    }
    private async void LoadGame()
    {
        _sceneSwitch.StartLoadLastLevel();
        await DeactivateGaragePanel();
        _sceneSwitch.EndLoadingSceneAsync();
    }
    private async void ActivateSettingsPanel()
    {
        _settingsPanel.FrameBackground.raycastTarget = true;
        _rectTransformPanelSettings.anchoredPosition = _valuesPanelActivator.StartPositionPanelSettings;
        _settingsPanel.gameObject.SetActive(true);
        _settingsPanel.FrameBackground.gameObject.SetActive(true);
        _settingsPanel.FrameBackground.color  = _valuesPanelActivator.DefaultColorMinAlpha;
        _settingsPanel.ButtonExit.onClick.AddListener(() =>
        {
            _audioHandlerUI.PlayClick();
            DeactivateSettingsPanel();
        });
        _startMenuPanel.ButtonSettings.onClick.RemoveAllListeners();
        
        _settingsPanel.ButtonMusic.onClick.AddListener(() =>
        {
            _audioSettingSwitch.MusicSwitch();
            _audioHandlerUI.PlayClick();
        });
        _settingsPanel.ButtonSound.onClick.AddListener(() =>
        {
            _audioSettingSwitch.SoundSwitch();
            _audioHandlerUI.PlayClick();
        });
        await MoveAndFadeWhenAll(_rectTransformPanelSettings.DOAnchorPos(_valuesPanelActivator.MediumPositionPanelSettings, _valuesPanelActivator.DefaultDurationFade, false).SetEase(Ease.InOutQuint).ToUniTask(),
            _settingsPanel.FrameBackground.DOFade(_valuesPanelActivator.ValueDarkenedBackground, _valuesPanelActivator.DefaultDurationFade).ToUniTask());
    }
    private async void DeactivateSettingsPanel()
    {
        _settingsPanel.ButtonExit.onClick.RemoveAllListeners();
        _settingsPanel.ButtonMusic.onClick.RemoveAllListeners();
        _settingsPanel.ButtonSound.onClick.RemoveAllListeners();
        _settingsPanel.FrameBackground.gameObject.GetComponent<Image>().raycastTarget = false;
        await MoveAndFadeWhenAll(_rectTransformPanelSettings.DOAnchorPos(_valuesPanelActivator.EndPositionPanelSettings, _valuesPanelActivator.DefaultDurationFade, false)
                .SetEase(Ease.InOutQuint)
                .OnComplete((
                    () =>
                    {
                        _settingsPanel.gameObject.SetActive(false);
                        _startMenuPanel.ButtonSettings.onClick.AddListener(() =>
                        {
                            _audioHandlerUI.PlayClick();
                            ActivateSettingsPanel();
                        });
                    })).ToUniTask(),
            _settingsPanel.FrameBackground
                .DOFade(_valuesPanelActivator.ValueFadeLightBackground, _valuesPanelActivator.DefaultDurationFade)
                .OnComplete((
                    () => { _settingsPanel.FrameBackground.gameObject.SetActive(false);}))
                .ToUniTask());
    }
    private async void SwitchStartMenuToMap()
    {
        await DeactivateStartMenuPanel();
        await ActivateMapPanel();
    }

    private async void SwitchMapToStartMenu()
    {
        await DeactivateMapPanel();
        await ActivateStartMenuPanel();
    }

    private async void SwitchMapToGarage()
    {
        await DeactivateMapPanel();
        await ActivateGaragePanel();
    }

    private async void SwitchGarageToMap()
    {
        await DeactivateGaragePanel();
        await ActivateMapPanel();
    }

    private void EnableDarkBackground()
    {
        _background.gameObject.SetActive(true);
    }

    private void DisableDarkBackground()
    {
        _background.gameObject.SetActive(false);
    }

    private void EnableRaycastTargetDarkBackground()
    {
        _background.raycastTarget = true;
    }

    private void DisableRaycastTargetDarkBackground()
    {
        _background.raycastTarget = false;
    }

    private void TryCancelFadeOperation()
    {
        _cancellationTokenSource?.Cancel();
    }

    private void InitCancellationTokenSource()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private float CalculateDuration(float currentAlphaValue)
    {
        if (currentAlphaValue == _valuesPanelActivator.ValueFadeLightBackground)
        {
            return _valuesPanelActivator.DefaultDurationFade;
        }
        else
        {
            return _valuesPanelActivator.DefaultDurationFade / (_valuesPanelActivator.ValueFadeBlackBackground / (_valuesPanelActivator.ValueFadeBlackBackground - currentAlphaValue));
        }
    }

    private UniTask FadeWithCancellation()
    {
        return _background.DOFade(_valuesPanelActivator.ValueFadeLightBackground, _valuesPanelActivator.DefaultDurationFade).WithCancellation(_cancellationTokenSource.Token);
    }

    private UniTask Fade()
    {
        return _background.DOFade(_valuesPanelActivator.ValueFadeBlackBackground, CalculateDuration(_background.color.a)).ToUniTask();
    }

    private UniTask MoveAndFadeWhenAll(UniTask movePanel, UniTask fadeFrame)
    {
        UniTask[] tasks = {movePanel, fadeFrame};
        UniTask whenAll = UniTask.WhenAll(tasks);
        return whenAll;
    }

    private async UniTask Deactivate()
    {
        TryCancelFadeOperation();
        EnableDarkBackground();
        EnableRaycastTargetDarkBackground();
        // InitCancellationTokenSource();
        await Fade();
        // _cancellationTokenSource.Cancel();
    }

    private async UniTask Activate()
    {
        EnableDarkBackground();
        DisableRaycastTargetDarkBackground();
        InitCancellationTokenSource();
        await FadeWithCancellation();
        _cancellationTokenSource = null;
        DisableDarkBackground();
    }
}
