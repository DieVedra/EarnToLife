using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;

public class NotificationsHandler
{
    private readonly float _delay = 1f;
    private readonly Queue<Notification> _messageQueue;
    private readonly TextMeshProUGUI _notificationsText;
    private readonly NotificationsProvider _notificationsProvider;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    // private PoolBase<Notification> _notificationsPool;

    private bool _inProgressShowing = false;
    public NotificationsHandler(ViewUILevel viewUILevel, ResultsLevelProvider resultsLevelProvider, ReactiveCommand disposeCommand)
    {
        _messageQueue = new Queue<Notification>();
        _notificationsText = viewUILevel.NotificationsText;
        _notificationsProvider = resultsLevelProvider.NotificationsProvider;
        _notificationsProvider.OnShowNotification += AddToQueueNotifications;
        _notificationsProvider.OnShowNotificationWithDelay += AddToQueueNotificationsWithDelay;
        _notificationsText.gameObject.SetActive(false);
        disposeCommand.Subscribe(_ => { Dispose();});
    }
    private void Dispose()
    {
        _notificationsProvider.OnShowNotification -= AddToQueueNotifications;
        _notificationsProvider.OnShowNotificationWithDelay -= AddToQueueNotificationsWithDelay;
        _cancellationTokenSource.Cancel();
    }

    private void AddToQueueNotifications(string text)
    {
        AddToQueueNotificationsWithDelay(text);
    }
    private void AddToQueueNotificationsWithDelay(string text, bool delay = false)
    {
        // Debug.Log($"AddToQueue {text}");
        _messageQueue.Enqueue(CreateNotification());
        TryShow(text, delay).Forget();
    }

    private async UniTaskVoid TryShow(string text, bool delay)
    {
        if (_inProgressShowing == false)
        {
            _inProgressShowing = true;

            while (_inProgressShowing == true)
            { 
                await _messageQueue.Dequeue().ShowNotification(text, delay);
                if (_messageQueue.Count == 0)
                {
                    _inProgressShowing = false;
                }
            }
        }
    }
    private Notification CreateNotification()
    {
        return new Notification(_notificationsText, _cancellationTokenSource, _delay);
    }
}
