using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

/*[ExecuteInEditMode]*/
public class test1 : MonoBehaviour
{
    public bool pause = false;
    private CancellationTokenSource _cancellationTokenSource;
    [Button("start timer")]
    private async void ff()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        Debug.Log($"Started Timer");

        await UniTask.Delay(TimeSpan.FromSeconds(5f), ignoreTimeScale: false, cancellationToken: _cancellationTokenSource.Token);
        
        Debug.Log($"Timer Ended");
    }
    [Button("pause")]
    private void c()
    {
        pause = true; 
    }
    [Button("Unpause")]
    private void b()
    {
        pause = false;
    }
    [Button("Stop")]
    private void n()
    {
        pause = false;
    }
}
