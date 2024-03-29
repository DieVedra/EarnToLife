using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

public class NotificationsHandler
{
    private readonly float _duration = 2f;
    private Queue<string> _operations;
    private TextMeshProUGUI _notificationsText;
    private NotificationsProvider _notificationsProvider;
    private bool _inProgressShowing = false;
    public NotificationsHandler(ViewUILevel viewUILevel, ResultsLevelProvider resultsLevelProvider, ReactiveCommand disposeCommand)
    {
        _operations = new Queue<string>();
        _notificationsText = viewUILevel.NotificationsText;
        _notificationsProvider = resultsLevelProvider.NotificationsProvider;
        _notificationsProvider.OnShowNotification += AddToQueueNotifications;
        _notificationsText.gameObject.SetActive(false);
        disposeCommand.Subscribe(_ => { Dispose();});
    }
    private void Dispose()
    {
        _notificationsProvider.OnShowNotification -= AddToQueueNotifications;
    }
    private void AddToQueueNotifications(string text)
    {
        _operations.Enqueue(text);
        if (_inProgressShowing == false)
        {
            _inProgressShowing = true;
            ShowNotifications();
        }
    }

    private async void ShowNotifications()
    {
        _notificationsText.gameObject.SetActive(true);

        _notificationsText.alpha = 1f;
        _notificationsText.text = _operations.Dequeue();
        await _notificationsText.DOFade(0f, _duration).ToUniTask();
        if (_operations.Count > 0)
        {
            ShowNotifications();
        }
        else
        {
            _inProgressShowing = false;
            _notificationsText.gameObject.SetActive(false);

        }
    }
}
