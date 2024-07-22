using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public class NotificationsHandler
{
    private readonly TextMeshProUGUI _notificationPrefab;
    private readonly float _delay = 1f;
    private readonly Vector2 _textPosition;
    private readonly Transform _textParent;
    private readonly Queue<Notification> _messageQueue;
    private readonly NotificationsProvider _notificationsProvider;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly NotificationWithDelay _notificationWithDelay;
    
    private readonly Spawner _spawner;
    private PoolBase<TextMeshProUGUI> _notificationsTextsPool;

    private bool _inProgressShowing = false;
    public NotificationsHandler(ViewUILevel viewUILevel, ResultsLevelProvider resultsLevelProvider, TextMeshProUGUI notificationPrefab, ReactiveCommand disposeCommand)
    {
        _notificationPrefab = notificationPrefab;
        _textParent = viewUILevel.NotificationsTextsParent;
        _spawner = new Spawner();
        _notificationsTextsPool = new PoolBase<TextMeshProUGUI>(CreateNotificationText, GetAction, ReturnAction, 3);

        _messageQueue = new Queue<Notification>();
        _notificationsProvider = resultsLevelProvider.NotificationsProvider;
        _notificationWithDelay = new NotificationWithDelay(_notificationsTextsPool.Get(), _cancellationTokenSource, _delay);
        _notificationsProvider.OnShowNotification += AddToQueueNotifications;
        _notificationsProvider.OnShowNotificationExplosion += AddToQueueNotificationsWithPos;
        _notificationsProvider.OnShowNotificationWithDelay += AddToQueueNotificationsWithDelay;
        disposeCommand.Subscribe(_ => { Dispose();});
    }
    private void Dispose()
    {
        _notificationsProvider.OnShowNotification -= AddToQueueNotifications;
        _notificationsProvider.OnShowNotificationExplosion -= AddToQueueNotificationsWithPos;
        _notificationsProvider.OnShowNotificationWithDelay -= AddToQueueNotificationsWithDelay;
        _cancellationTokenSource.Cancel();
    }
    private void AddToQueueNotificationsWithPos(string text, Vector2 position)
    {
        ShowNotificationsWithPos(text, position).Forget();
    }

    private async UniTaskVoid ShowNotificationsWithPos(string text, Vector2 position)
    {
        Notification notification = CreateNotificationWithPosition(position);
        await notification.ShowNotification(text);
        _notificationsTextsPool.Return(notification.NotificationsText);
    }
    private void AddToQueueNotificationsWithDelay(string text)
    {
        AddNotification(_notificationWithDelay, text);

    }
    private void AddToQueueNotifications(string text)
    {
        AddNotification(CreateNotificationDefault(), text);
    }
    private void AddNotification(Notification notification, string text)
    {
        _messageQueue.Enqueue(notification);
        TryShow(text).Forget();
    }

    private async UniTaskVoid TryShow(string text)
    {
        if (_inProgressShowing == false)
        {
            _inProgressShowing = true;

            while (_inProgressShowing == true)
            {
                Notification notification = _messageQueue.Dequeue();
                await notification.ShowNotification(text);
                _notificationsTextsPool.Return(notification.NotificationsText);
                if (_messageQueue.Count == 0)
                {
                    _inProgressShowing = false;
                }
            }
        }
    }

    private TextMeshProUGUI CreateNotificationText()
    {
        return _spawner.Spawn(_notificationPrefab, _textParent, _textParent);
    }

    private void GetAction(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(true);
    }
    private void ReturnAction(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(false);
        text.transform.position = _textParent.position;
    }

    private NotificationWithPosition CreateNotificationWithPosition(Vector2 position)
    {
        return new NotificationWithPosition(_notificationsTextsPool.Get(), _cancellationTokenSource, position);
    }

    private Notification CreateNotificationDefault()
    {
        return new Notification(_notificationsTextsPool.Get(), _cancellationTokenSource);
    }
}
