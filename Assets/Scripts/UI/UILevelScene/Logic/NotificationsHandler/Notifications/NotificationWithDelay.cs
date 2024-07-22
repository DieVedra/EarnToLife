using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;

public class NotificationWithDelay : Notification
{
    private readonly float _delay;

    public NotificationWithDelay(TextMeshProUGUI notificationsText, CancellationTokenSource cancellationTokenSource, float delay)
        : base(notificationsText, cancellationTokenSource)
    {
        _delay = delay;
    }
    public override async UniTask ShowNotification(string text)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delay), ignoreTimeScale: true, cancellationToken: CancellationTokenSource.Token);
        NotificationsText.gameObject.SetActive(true);
        NotificationsText.alpha = 1f;
        NotificationsText.text = text;
        await UniTask.WhenAll(GetAnimation());
        NotificationsText.fontSize = FontSizeDefault;
    }
}