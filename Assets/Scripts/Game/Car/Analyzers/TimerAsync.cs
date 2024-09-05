using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class TimerAsync
{
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
     public bool TimerActive { get; private set; }

     public async UniTaskVoid TryStartTimer(Action onTimerEndOperation, float delay)
     {
         if (TimerActive == false)
         {
             TimerActive = true;
             await UniTask.Delay(TimeSpan.FromSeconds(delay), ignoreTimeScale: false, cancellationToken: _cancellationTokenSource.Token);
             onTimerEndOperation?.Invoke();
             TimerActive = false;
         }
     }

     public void TryStopTimer()
     {
         if (TimerActive == true)
         {
             TimerActive = false;
             _cancellationTokenSource.Cancel();
         }
     }
}