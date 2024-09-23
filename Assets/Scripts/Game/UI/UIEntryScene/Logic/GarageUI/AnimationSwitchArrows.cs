using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSwitchArrows
{
    private readonly float _duration = 0.5f;
    private readonly float _alphaLow = 0f;
    private readonly float _alphaHight = 1f;
    private Image _imageArrowRight;
    private Image _imageArrowLeft;
    private CancellationTokenSource _cancellationTokenSource;
    
    public AnimationSwitchArrows(Image imageArrowRight, Image imageArrowLeft)
    {
        _imageArrowRight = imageArrowRight;
        _imageArrowLeft = imageArrowLeft;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public async UniTask SwitchRight()
    {
        await FadeImage(_imageArrowRight);
    }
    public async UniTask SwitchLeft()
    {
        await FadeImage(_imageArrowLeft);
    }
    public void UnfadeArrowRight()
    {
        UnfadeArrow(_imageArrowRight);
    }
    public void UnfadeArrowLeft()
    {
        UnfadeArrow(_imageArrowLeft);
    }

    private void UnfadeArrow(Image imageArrow)
    {
        imageArrow.color = SetAlphaTransparent(imageArrow.color);
        imageArrow.DOFade(_alphaHight, _duration).WithCancellation(_cancellationTokenSource.Token);
    }
    private Color SetAlphaTransparent(Color color)
    {
        return new Color(color.r, color.g, color.b, _alphaLow);
    }
    private async UniTask FadeImage(Image image)
    {
        await image.DOFade(_alphaLow, _duration).WithCancellation(_cancellationTokenSource.Token);
    }
}
