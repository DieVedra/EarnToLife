using System.Collections;
using System.Collections.Generic;
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
    
    public AnimationSwitchArrows(Image imageArrowRight, Image imageArrowLeft)
    {
        _imageArrowRight = imageArrowRight;
        _imageArrowLeft = imageArrowLeft;
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
        imageArrow.DOFade(_alphaHight, _duration).SetEase(Ease.InExpo).ToUniTask();
    }
    private Color SetAlphaTransparent(Color color)
    {
        return new Color(color.r, color.g, color.b, _alphaLow);
    }
    private async UniTask FadeImage(Image image)
    {
        await image.DOFade(_alphaLow, _duration).SetEase(Ease.InExpo).ToUniTask();
    }
}
