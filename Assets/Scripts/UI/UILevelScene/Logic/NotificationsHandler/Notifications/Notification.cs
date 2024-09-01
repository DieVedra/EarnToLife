﻿
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

public class Notification
{
    private readonly float _fontSizeMultiplier = 0.9f;
    private readonly float _fontSizeDefault;
    private readonly float _fontSizeEndAnimation;
    private readonly float _duration = 2f;
    protected readonly CancellationTokenSource CancellationTokenSource;
    protected readonly float FontSizeDefault;
    public TextMeshProUGUI NotificationsText { get; private set; }

    public Notification(TextMeshProUGUI notificationsText, CancellationTokenSource cancellationTokenSource, float fontSizeDefault = 0f)
    {
        FontSizeDefault = notificationsText.fontSize;
        NotificationsText = notificationsText;
        if (fontSizeDefault == 0)
        {
            _fontSizeDefault = NotificationsText.fontSize;
        }
        else
        {
            _fontSizeDefault = fontSizeDefault;
            NotificationsText.fontSize = fontSizeDefault;
        }

        _fontSizeEndAnimation = _fontSizeDefault * _fontSizeMultiplier;
        CancellationTokenSource = cancellationTokenSource;
    }
    public virtual async UniTask ShowNotification(string text)
    {
        NotificationsText.gameObject.SetActive(true);
        NotificationsText.alpha = 1f;
        NotificationsText.text = text;
        await UniTask.WhenAll(GetAnimation());
        NotificationsText.fontSize = FontSizeDefault;
    }
    protected virtual UniTask[] GetAnimation()
    {
        return new[]
        {
            DOTween.To(() => NotificationsText.fontSize, x => NotificationsText.fontSize = x, _fontSizeEndAnimation, _duration)
                .SetUpdate(true)
                .SetEase(Ease.OutCubic).WithCancellation(CancellationTokenSource.Token),
            NotificationsText.DOFade(0f, _duration).SetUpdate(true).SetEase(Ease.InExpo)
                .WithCancellation(CancellationTokenSource.Token)
        };
    }
}