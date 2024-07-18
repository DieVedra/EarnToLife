
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

public class Notification
{
    private readonly float _fontSizeDefault;
    private readonly float _fontSizeMax = 180f;
    private readonly float _duration = 2f;
    private readonly TextMeshProUGUI _notificationsText;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly float _delay;

    public Notification(TextMeshProUGUI notificationsText, CancellationTokenSource cancellationTokenSource, float delay)
    {
        _fontSizeDefault = notificationsText.fontSize;
        _notificationsText = notificationsText;
        _cancellationTokenSource = cancellationTokenSource;
        _delay = delay;
    }
    public async UniTask ShowNotification(string text, bool delay = false)
    {
        if (delay == true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), ignoreTimeScale: true, cancellationToken: _cancellationTokenSource.Token);
        }
        _notificationsText.gameObject.SetActive(true);
        _notificationsText.alpha = 1f;
        _notificationsText.text = text;
        await UniTask.WhenAll(GetAnimation());
        _notificationsText.fontSize = _fontSizeDefault;
    }
    private UniTask[] GetAnimation()
    {
        return new[]
        {
            DOTween.To(() => _notificationsText.fontSize, x => _notificationsText.fontSize = x, _fontSizeMax, _duration)
                .SetUpdate(true)
                .SetEase(Ease.OutCubic).WithCancellation(_cancellationTokenSource.Token),
            _notificationsText.DOFade(0f, _duration).SetUpdate(true).SetEase(Ease.InExpo)
                .WithCancellation(_cancellationTokenSource.Token)
        };
    }
}