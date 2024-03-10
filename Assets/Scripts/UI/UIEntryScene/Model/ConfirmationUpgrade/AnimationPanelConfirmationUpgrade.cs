using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPanelConfirmationUpgrade
{
    private readonly Vector2 _hidePositionPanel = new Vector2(0f, 800f);
    private readonly Vector2 _unhidePositionPanel = new Vector2(0f, 85f);
    private readonly Color _hideColor = Color.clear;
    private readonly float _hidePanelAlpha = 0f;
    private readonly float _unhidePanelAlpha = 0.4f;
    private readonly float _duration = 0.5f;
    private RectTransform _rectTransformPanel;
    private Image _frameBackground;
    public AnimationPanelConfirmationUpgrade(RectTransform rectTransformPanel, Image frameBackground)
    {
        _rectTransformPanel = rectTransformPanel;
        _frameBackground = frameBackground;
        InitFrameBackground();
    }
    public async UniTask HidePanel()
    {
        await MoveAndFadePanelWhenAll(
            _rectTransformPanel.DOAnchorPos(_hidePositionPanel, _duration).SetEase(Ease.OutBack).ToUniTask(),
            _frameBackground.DOFade(_hidePanelAlpha, _duration).ToUniTask());
        DisableRaycastTarget();
        _frameBackground.gameObject.SetActive(false);
        _rectTransformPanel.gameObject.SetActive(false);
    }

    public void UnhidePanel()
    {
        _frameBackground.color = _hideColor;
        EnableRaycastTarget();
        _frameBackground.gameObject.SetActive(true);
        _rectTransformPanel.localPosition = _hidePositionPanel;
        _rectTransformPanel.gameObject.SetActive(true);
        MoveAndFadePanelWhenAll(
            _rectTransformPanel.DOAnchorPos(_unhidePositionPanel, _duration).SetEase(Ease.OutBack).ToUniTask(),
            _frameBackground.DOFade(_unhidePanelAlpha, _duration).ToUniTask());
    }
    private UniTask MoveAndFadePanelWhenAll(UniTask movePanel, UniTask fadeFrame)
    {
        UniTask[] tasks = {movePanel, fadeFrame};
        UniTask whenAll = UniTask.WhenAll(tasks);
        return whenAll;
    }

    private void EnableRaycastTarget()
    {
        _frameBackground.raycastTarget = true;
    }
    private void DisableRaycastTarget()
    {
        _frameBackground.raycastTarget = false;
    }

    private void InitFrameBackground()
    {
        DisableRaycastTarget();
        _frameBackground.gameObject.SetActive(false);
    }
}
