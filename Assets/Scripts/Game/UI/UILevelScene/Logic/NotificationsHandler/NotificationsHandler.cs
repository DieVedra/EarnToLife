using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public class NotificationsHandler
{
    private readonly Vector2 _textPosition;
    private readonly Transform _textParent;
    private readonly NotificationsCreator _notificationsCreator;
    private readonly Queue<Notification> _messageQueue;
    private readonly NotificationsProvider _notificationsProvider;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly NotificationWithDelay _notificationWithDelay;
    
    private PoolBase<TextMeshProUGUI> _notificationsTextsPool;

    private bool _inProgressShowing = false;
    public NotificationsHandler(ViewUILevel viewUILevel, ResultsLevelProvider resultsLevelProvider, TextMeshProUGUI notificationPrefab, ReactiveCommand disposeCommand)
    {
        _textParent = viewUILevel.NotificationsTextsParent;
        _notificationsCreator = new NotificationsCreator(viewUILevel.NotificationsTextsParent, notificationPrefab);
        _notificationsTextsPool = new PoolBase<TextMeshProUGUI>(
            _notificationsCreator.CreateNotificationText, 
            _notificationsCreator.GetAction, 
            _notificationsCreator.ReturnAction, 3);

        _messageQueue = new Queue<Notification>();
        _notificationsProvider = resultsLevelProvider.NotificationsProvider;
        _notificationWithDelay = _notificationsCreator.CreateNotificationWithDelay(_notificationsTextsPool.Get());
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
        _notificationsCreator.Dispose();
    }
    private void AddToQueueNotificationsWithPos(string text, Vector2 position)
    {
        ShowNotificationsWithPos(text, position).Forget();
    }

    private async UniTaskVoid ShowNotificationsWithPos(string text, Vector2 position)
    {
        Notification notification = _notificationsCreator.CreateNotificationWithPosition(_notificationsTextsPool.Get(), position);
        await notification.ShowNotification(text);
        _notificationsTextsPool.Return(notification.NotificationsText);
    }
    private void AddToQueueNotificationsWithDelay(string text)
    {
        AddNotification(_notificationWithDelay, text);
    }
    private void AddToQueueNotifications(string text)
    {
        AddNotification(_notificationsCreator.CreateNotificationDefault(_notificationsTextsPool.Get()), text);
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
                notification.NotificationsText.transform.position = _textParent.position;
                await notification.ShowNotification(text);
                _notificationsTextsPool.Return(notification.NotificationsText);
                if (_messageQueue.Count == 0)
                {
                    _inProgressShowing = false;
                }
            }
        }
    }
}
