using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

public class AppearanceButtonsAnimation
{
    private readonly Vector2 _openPos = new Vector2(0f,-377f);
    private readonly Vector2 _hidePos = new Vector2(0f,-700f);
    private readonly float _hideValue = 0f;
    private readonly float _unhideValue = 1f;
    private readonly float _duration = 0.6f;
    private RectTransform _parentRectTransformButtonsUpgrade;
    private IReadOnlyList<CanvasGroup> _canvasGroupsButtonsUpgrade;
    private List<UniTask> _tasks;
    public AppearanceButtonsAnimation(IReadOnlyList<CanvasGroup> canvasGroupsButtonsUpgrade, RectTransform parentRectTransformButtonsUpgrade)
    {
        _canvasGroupsButtonsUpgrade = canvasGroupsButtonsUpgrade;
        _parentRectTransformButtonsUpgrade = parentRectTransformButtonsUpgrade;
    }
    public void Unhide()
    {
        _parentRectTransformButtonsUpgrade.localPosition = _hidePos;
        _parentRectTransformButtonsUpgrade.gameObject.SetActive(true);
        for (int i = 0; i < _canvasGroupsButtonsUpgrade.Count; i++)
        {
            _canvasGroupsButtonsUpgrade[i].alpha = _hideValue;
        }
        _tasks = new List<UniTask>();
        _tasks.Add(_parentRectTransformButtonsUpgrade.DOAnchorPos(_openPos, _duration).SetEase(Ease.InOutBack).ToUniTask());
        for (int i = 0; i < _canvasGroupsButtonsUpgrade.Count; i++)
        {
            _tasks.Add(_canvasGroupsButtonsUpgrade[i].DOFade(_unhideValue, _duration).SetEase(Ease.InQuart).ToUniTask());
        }
    }
    public async UniTask Hide()
    {
        _tasks = new List<UniTask>();
        _tasks.Add(_parentRectTransformButtonsUpgrade.DOAnchorPos(_hidePos, _duration).SetEase(Ease.InOutBack).ToUniTask());
        for (int i = 0; i < _canvasGroupsButtonsUpgrade.Count; i++)
        {
            _tasks.Add(_canvasGroupsButtonsUpgrade[i].DOFade(_hideValue, _duration).SetEase(Ease.OutQuart).ToUniTask());
        }
        await UniTask.WhenAll(_tasks);
        _parentRectTransformButtonsUpgrade.gameObject.SetActive(true);
    }
}
