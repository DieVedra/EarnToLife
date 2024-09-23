using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelsHandler
{
    private ValuesPanelHandler _valuesPanelHandler;
    private Button _buttonPause;
    private SceneSwitch _sceneSwitch;
    private PanelPause _panelPause;
    private PanelScore _panelScore;
    private ViewUILevel _viewUILevel;
    private PausePanelButtonsHandler _pausePanelButtonsHandler;
    private ResultsLevelProvider _resultsLevelProvider;
    private ButtonsControl _buttonsControl;
    private Image _background;
    private Image _frameBackground;
    private GamePause _gamePause;
    private ResultsLevelHandler _resultsLevelHandler;
    private AudioSettingSwitch _audioSettingSwitch;
    private readonly IconLoadHandler _iconLoadHandler;
    private IClickAudio _audioClickAudioHandler;
    
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public PanelsHandler(ViewUILevel viewUILevel, SceneSwitch sceneSwitch, PausePanelButtonsHandler pausePanelButtonsHandler,
        ButtonsControl buttonsControl, GamePause gamePause, ResultsLevelProvider resultsLevel,
        ResultsLevelHandler resultsLevelUI, AudioSettingSwitch audioSettingSwitch, IconLoadHandler iconLoadHandler, IClickAudio audioClickAudioHandler, ReactiveCommand disposeCommand)
    {
        _valuesPanelHandler = new ValuesPanelHandler();
        _buttonPause = viewUILevel.ButtonPause;
        _sceneSwitch = sceneSwitch;
        _panelPause = viewUILevel.PanelPause;
        _panelScore = viewUILevel.PanelScore;
        _pausePanelButtonsHandler = pausePanelButtonsHandler;
        _buttonsControl = buttonsControl;
        _viewUILevel = viewUILevel;
        _background = viewUILevel.Background;
        _frameBackground = viewUILevel.FrameBackground;
        _gamePause = gamePause;
        _resultsLevelProvider = resultsLevel;
        _resultsLevelHandler = resultsLevelUI;
        _audioSettingSwitch = audioSettingSwitch;
        _iconLoadHandler = iconLoadHandler;
        _audioClickAudioHandler = audioClickAudioHandler;
        _resultsLevelProvider.OnOutCalculateResultsLevel += ActivateScorePanel;
        disposeCommand.Subscribe(_ => { Dispose();});
        StartInit();
    }
    private void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    private async void StartInit()
    {
        FrameBackgroundDisable();
        _background.color = Color.black;
        DisableRaycastTargetDarkBackground();
        _background.gameObject.SetActive(true);
        _viewUILevel.gameObject.SetActive(true);
        _buttonsControl?.Activate();
        _buttonPause.onClick.AddListener(ActivatePausePanel);

        await _background.DOFade(
            _valuesPanelHandler.ClearBackground, 
            _valuesPanelHandler.BackgroundDurationFade).WithCancellation(_cancellationTokenSource.Token);
        _background.gameObject.SetActive(false);
    }
    private async void ActivatePausePanel()
    {
        _audioClickAudioHandler.PlayClick();
        FrameBackgroundEnable();
        _panelPause.RectTransform.localPosition = _valuesPanelHandler.StartPositionPanel;
        _panelPause.gameObject.SetActive(true);
        SubscribePausePanelButtons();
        _buttonPause.onClick.RemoveAllListeners();
        _gamePause.SetPause();
        await WhenAll(_panelPause.RectTransform.DOAnchorPos(
                _valuesPanelHandler.EndPositionPanel,
                _valuesPanelHandler.MovePanelDuration).SetEase(Ease.InOutQuint).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token),
            CreateScreenShot(),
            _frameBackground.DOColor(_valuesPanelHandler.DarkenedFrameBackgroundColor, _valuesPanelHandler.FrameBackgroundDurationFade).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token),
            DOTween.To(() => 0f, x => _frameBackground.material.SetFloat("_BlurAmount", x), 0.5f, _valuesPanelHandler.FrameBackgroundDurationFade).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token)
        );
    }
    private async void DeactivatePausePanel()
    {
        _audioClickAudioHandler.PlayClick();
        ButtonsPausePanelUnsubscribe();
        _frameBackground.raycastTarget = false;
        await WhenAll(_panelPause.RectTransform.DOAnchorPos(
                _valuesPanelHandler.StartPositionPanel,
                _valuesPanelHandler.MovePanelDuration).SetEase(Ease.InOutQuint).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token),
            _frameBackground.DOColor(_valuesPanelHandler.ClearFrameBackgroundColor, _valuesPanelHandler.FrameBackgroundDurationFade).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token),
            DOTween.To(() => 0.5f, x => _frameBackground.material.SetFloat("_BlurAmount", x), 0f, _valuesPanelHandler.FrameBackgroundDurationFade).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token));
        
        _gamePause.AbortPause();
        _frameBackground.gameObject.SetActive(false);
        _panelPause.gameObject.SetActive(false);
        _buttonPause.onClick.AddListener(ActivatePausePanel);
    }
    private void DeactivatePausePanelAndRestart()
    {
        _sceneSwitch.StartLoadLastLevel();
        DeactivatePausePanelToLoad();
    }
    private void DeactivatePausePanelAndLoadGarage()
    {
        _sceneSwitch.StartLoadGarage();
        DeactivatePausePanelToLoad();
    }

    private async void DeactivatePausePanelToLoad()
    {
        _audioClickAudioHandler.PlayClick();
        EnableRaycastTargetDarkBackground();
        ButtonsPausePanelUnsubscribe();
        _background.gameObject.SetActive(true);
        await _background.DOFade(
            _valuesPanelHandler.BlackBackground,
            _valuesPanelHandler.BackgroundDurationFade).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token);
        _iconLoadHandler.ShowIconLoad();
        _panelPause.gameObject.SetActive(false);
        _gamePause.AbortPauseForLoad();
        EndLoadNextScene();
    }
    private async void ActivateScorePanel(ResultsLevel results, ResultsLevel lastResults)
    {
        FrameBackgroundEnable();
        _panelScore.RectTransform.localPosition = _valuesPanelHandler.StartPositionPanel;
        _panelScore.ButtonGarage.onClick.AddListener(DeactivateScorePanelAndLoadGarage);
        _panelScore.gameObject.SetActive(true);
        _sceneSwitch.StartLoadGarage();
        _resultsLevelHandler.DisplayOutResultsLevel(results, lastResults);
        await WhenAll(
            _panelScore.RectTransform.DOAnchorPos(
                _valuesPanelHandler.EndPositionPanel,
                _valuesPanelHandler.MovePanelDuration).SetEase(Ease.InOutQuint).WithCancellation(_cancellationTokenSource.Token),
            CreateScreenShot(),
            _frameBackground.DOColor(_valuesPanelHandler.DarkenedFrameBackgroundColor, _valuesPanelHandler.FrameBackgroundDurationFade).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token),
            DOTween.To(() => 0f, x => _frameBackground.material.SetFloat("_BlurAmount", x), 0.5f, _valuesPanelHandler.FrameBackgroundDurationFade).SetUpdate(true).WithCancellation(_cancellationTokenSource.Token));
    }

    private async void DeactivateScorePanelAndLoadGarage()
    {
        _audioClickAudioHandler.PlayClick();
        _background.gameObject.SetActive(true);
        _panelScore.ButtonGarage.onClick.RemoveAllListeners();

        EnableRaycastTargetDarkBackground();
        await _background.DOFade(_valuesPanelHandler.BlackBackground, _valuesPanelHandler.BackgroundDurationFade).WithCancellation(_cancellationTokenSource.Token);
        _iconLoadHandler.ShowIconLoad();
        EndLoadNextScene();
    }

    private void EnableRaycastTargetDarkBackground()
    {
        _background.raycastTarget = true;
    }

    private void DisableRaycastTargetDarkBackground()
    {
        _background.raycastTarget = false;
    }

    private UniTask WhenAll(params UniTask[] tasks)
    {
        UniTask whenAll = UniTask.WhenAll(tasks);
        return whenAll;
    }

    private void FrameBackgroundEnable()
    {
        _frameBackground.gameObject.SetActive(true);
        _frameBackground.color = Color.clear;
        _frameBackground.raycastTarget = true;
    }

    private void FrameBackgroundDisable()
    {
        _frameBackground.gameObject.SetActive(false);
        _frameBackground.color = Color.clear;
        _frameBackground.raycastTarget = false;
    }

    private void EndLoadNextScene()
    {
        _resultsLevelProvider.OnOutCalculateResultsLevel -= ActivateScorePanel;
        _buttonsControl?.Deactivate();
        _sceneSwitch.EndLoadingSceneAsync();
    }

    private void SubscribePausePanelButtons()
    {
        _pausePanelButtonsHandler.ButtonClose.onClick.AddListener(DeactivatePausePanel);
        _pausePanelButtonsHandler.ButtonRestart.onClick.AddListener(DeactivatePausePanelAndRestart);
        _pausePanelButtonsHandler.ButtonGarage.onClick.AddListener(DeactivatePausePanelAndLoadGarage);
        _pausePanelButtonsHandler.ButtonMusic.onClick.AddListener(() =>
        {
            _audioClickAudioHandler.PlayClick();
            _audioSettingSwitch.MusicSwitch();
        });
        _pausePanelButtonsHandler.ButtonSound.onClick.AddListener(() =>
        {
            _audioSettingSwitch.SoundSwitch();
            _audioClickAudioHandler.PlayClick();
        });
    }

    private void ButtonsPausePanelUnsubscribe()
    {
        _pausePanelButtonsHandler.Unsubscribe();
    }
    private async UniTask CreateScreenShot()
    {
        await UniTask.WaitForEndOfFrame(_frameBackground);
        Texture2D texture2d = ScreenCapture.CaptureScreenshotAsTexture();
        _frameBackground.sprite = Sprite.Create(texture2d,
            new Rect(0f, 0f, texture2d.width, texture2d.height),
            new Vector2(0.5f, 0.5f));
    }
}
