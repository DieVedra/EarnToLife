using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class test3 : MonoBehaviour
{
    private CancellationTokenSource _cancellationTokenSource;
    public Image _image;
    public float duration;
    public SpriteRenderer SpriteRenderer;
    private Vector3 _rotateValue = new Vector3(0f,0f,0f);
    // private CompositeDisposable _compositeDisposable;


    private void Start()
    {
        Time.timeScale = 1f;
    }

    [Button]
    private void b()
    {
        EngineGame();
    }
    private async void EngineGame()
    {
        await delay();
        a();
        await delay();
        e();
    }
    private async UniTask delay()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }
    [Button]
    private void a()
    {
        EngineGame();
        _cancellationTokenSource  = new CancellationTokenSource();
        
        _image.rectTransform.DORotate(new Vector3(0f,0f, -180f), duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental)
            .SetUpdate(true).WithCancellation(_cancellationTokenSource.Token);




        // c().Forget();
        // _compositeDisposable = new CompositeDisposable();
        // Observable.EveryFixedUpdate().Subscribe(_ =>
        // {
        //     _rotateValue.z += dfdf;
        //     // transform.rotation = Quaternion.Euler(_rotateValue);
        //     Debug.Log(_rotateValue.z);
        //     
        //     
        //     transform.Rotate(_rotateValue);
        //     
        // }).AddTo(_compositeDisposable);
    }
    [Button]
    private void e()
    {
        _cancellationTokenSource.Cancel();
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
}
