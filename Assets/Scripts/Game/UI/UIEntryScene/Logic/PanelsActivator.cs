using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelsActivator
{
    private readonly ValuesPanelActivator _valuesPanelActivator;
    private readonly StartMenuPanel _startMenuPanel;
    private readonly SettingsPanel _settingsPanel;
    private readonly MapPanel _mapPanel;
    private readonly GaragePanel _garagePanel;
    private readonly Image _background;
    private readonly SpriteRenderer _startMenuBackground;
    private readonly Map _map;
    private readonly Garage _garage;
    private readonly GarageUI _garageUI;
    private readonly SceneSwitch _sceneSwitch;
    private readonly GameData _gameData;
    private readonly MapPanelHandler _mapPanelHandler;
    private readonly RectTransform _rectTransformPanelSettings;
    private readonly AudioHandlerUI _audioHandlerUI;
    private readonly AudioSettingSwitch _audioSettingSwitch;
    private readonly IconLoadHandler _iconLoadHandler;

    private CancellationTokenSource _cancellationTokenSource;

    public PanelsActivator(ViewEntryScene viewEntryScene, GarageUI buttonsSelectLotCar, SceneSwitch sceneSwitch,
        Image background, SpriteRenderer startMenuBackground, Map map, Garage garage, GameData gameData, MapPanelHandler mapPanelHandler,
        AudioHandlerUI audioHandlerUI, AudioSettingSwitch audioSettingSwitch, IconLoadHandler iconLoadHandler)
    {
        _valuesPanelActivator = new ValuesPanelActivator();
        _audioSettingSwitch = audioSettingSwitch;
        _iconLoadHandler = iconLoadHandler;
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
        _map = map;
        _garage = garage;
        _rectTransformPanelSettings = _settingsPanel.GetComponent<RectTransform>();
        StartGame();
    }

    private void StartGame()
    {
        _startMenuPanel.PanelSettings.gameObject.SetActive(false);
        switch (_gameData.StartPanelInMenu)
        {
            case PanelsInMenu.StartPanel:
                SetActivateMapPanel(false);
                SetActivateGaragePanel(false);
                _garageUI.Deactivate();
                ActivateStartMenuPanel().Forget();
                break;

            case PanelsInMenu.MapPanel:
                SetActivateStartPanel(false);
                SetActivateGaragePanel(false);
                _garageUI.Deactivate();
                ActivateMapPanel().Forget();
                break;
            
            case PanelsInMenu.GaragePanel:
                SetActivateStartPanel(false);
                SetActivateMapPanel(false);
                ActivateGaragePanel().Forget();
                break;
        }
    }
    
    private async UniTask ActivateStartMenuPanel()
    {
        SetActivateStartPanel(true);
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
        SetActivateStartPanel(false);
    }
    private async UniTask ActivateMapPanel()
    {
        SetActivateMapPanel(true);
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
        SetActivateMapPanel(false);
        _mapPanelHandler.MapDisable();
    }
    private async UniTask ActivateGaragePanel()
    {
        SetActivateGaragePanel(true);
        _garagePanel.ButtonBackToMap.onClick.AddListener(()=>
        {
            _audioHandlerUI.PlayClick();
            SwitchGarageToMap();
        });
        _garagePanel.ButtonGO.onClick.AddListener((() =>
        {
            _audioHandlerUI.PlayClick();
            EngineGame();
        }));
        _garageUI.Activate();
        await Activate();
        _garage.GarageLight.LampOn();
    }

    private async UniTask DeactivateGaragePanel()
    {
        _garagePanel.ButtonBackToMap.onClick.RemoveAllListeners();
        _garagePanel.ButtonGO.onClick.RemoveAllListeners();
        await _garage.GarageLight.LampOff();
        await MoveAndFadeWhenAll(_garageUI.DeactivateUpgradeButtons(), Deactivate());
        _garageUI.Deactivate();
        SetActivateGaragePanel(false);
    }
    private async void EngineGame()
    {
        await DeactivateGaragePanel();
        _sceneSwitch.StartLoadLastLevel();
        _iconLoadHandler.ShowIconLoad();
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
            return _valuesPanelActivator.DefaultDurationFade / 
                   (_valuesPanelActivator.ValueFadeBlackBackground / (_valuesPanelActivator.ValueFadeBlackBackground - currentAlphaValue));
        }
    }

    private UniTask FadeWithCancellation()
    {
        return _background.DOFade(_valuesPanelActivator.ValueFadeLightBackground, _valuesPanelActivator.DefaultDurationFade)
            .WithCancellation(_cancellationTokenSource.Token);
    }

    private UniTask Fade()
    {
        return _background.DOFade(_valuesPanelActivator.ValueFadeBlackBackground, CalculateDuration(_background.color.a))
            .ToUniTask();
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
        await Fade();
    }

    private async UniTask Activate()
    {
        EnableDarkBackground();
        DisableRaycastTargetDarkBackground();
        InitCancellationTokenSource();
        await FadeWithCancellation();
        DisableDarkBackground();
        _cancellationTokenSource = null;
    }

    private void SetActivateStartPanel(bool key)
    {
        _startMenuPanel.gameObject.SetActive(key); 
        _startMenuBackground.gameObject.SetActive(key);
    }
    private void SetActivateMapPanel(bool key)
    {
        _mapPanel.gameObject.SetActive(key);
        _map.gameObject.SetActive(key);
    }
    
    private void SetActivateGaragePanel(bool key)
    {
        _garage.gameObject.SetActive(key);
        _garagePanel.gameObject.SetActive(key);
    }
}
