using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;

public class NotificationWithPosition : Notification
{
    private readonly Vector2 _positionExplosion;
    private bool _key;

    public NotificationWithPosition(TextMeshProUGUI notificationsText, CancellationTokenSource cancellationTokenSource, Vector2 positionExplosion)
        : base(notificationsText, cancellationTokenSource)
    {
        _positionExplosion = positionExplosion;
    }
    public override async UniTask ShowNotification(string text)
    {
        _key = true;
        GivePosition().Forget();
        NotificationsText.gameObject.SetActive(true);
        NotificationsText.alpha = 1f;
        NotificationsText.text = text;
        await UniTask.WhenAll(GetAnimation());
        NotificationsText.fontSize = FontSizeDefault;
        _key = false;
    }

    private async UniTask GivePosition()
    {
        while (_key == true)
        {
            await UniTask.NextFrame(cancellationToken: CancellationTokenSource.Token);
            NotificationsText.transform.position = _positionExplosion;
        }
    }
}