using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class IconLoadHandler
{
    private readonly float _duration = 0.5f;
    private readonly Vector3 _rotateValue = new Vector3(0f,0f, -180f);
    private Image _iconLoad;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public IconLoadHandler(Image iconLoad)
    {
        _iconLoad = iconLoad;
        _iconLoad.gameObject.SetActive(false);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    
    public void ShowIconLoad()
    {
        _iconLoad.gameObject.SetActive(true);
        _iconLoad.rectTransform.DORotate(_rotateValue, _duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental)
            .SetUpdate(true).WithCancellation(_cancellationTokenSource.Token);
    }
}