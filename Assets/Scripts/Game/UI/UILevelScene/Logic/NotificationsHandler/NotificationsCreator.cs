using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class NotificationsCreator
{
    private readonly float _fontSizeDefaultNotificationWithPosition = 100f;
    private readonly float _delay = 1f;

    private readonly TextMeshProUGUI _notificationPrefab;
    private readonly Transform _textParent;

    private readonly Spawner _spawner;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public NotificationsCreator(Transform textParent, TextMeshProUGUI notificationPrefab)
    {
        _notificationPrefab = notificationPrefab;
        _textParent = textParent;
        _spawner = new Spawner();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public TextMeshProUGUI CreateNotificationText()
    {
        TextMeshProUGUI notification = _spawner.Spawn(_notificationPrefab, _textParent, _textParent);
        notification.gameObject.SetActive(false);
        return notification;
    }
    public NotificationWithPosition CreateNotificationWithPosition(TextMeshProUGUI text, Vector2 position)
    {
        return new NotificationWithPosition(text, _cancellationTokenSource, position, _fontSizeDefaultNotificationWithPosition);
    }

    public Notification CreateNotificationDefault(TextMeshProUGUI text)
    {
        return new Notification(text, _cancellationTokenSource);
    }
    public NotificationWithDelay CreateNotificationWithDelay(TextMeshProUGUI text)
    {
        return new NotificationWithDelay(text, _cancellationTokenSource, _delay);
    }
    public void GetAction(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(true);
    }
    public void ReturnAction(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(false);
    }
}