using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;

public class AnimationsUI
{
    private const float DURATION_ANIM_IN_SECONDS = 1f;
    private Image _image;
    private Sequence _sequence;
    private Tween _tweenDark;
    private Tween _tweenLight;
    public AnimationsUI(Image image)
    {
        _image = image;
    }
    public void PanelSwitchAnimation(Action operationOnStartDark, Action operationOnCompleteDark, Action operationOnStartLight, Action operationOnCompleteLight)
    {
        _tweenDark = DarkAnimationFade(operationOnStartDark, operationOnCompleteDark);
        _tweenLight = LightAnimationFade(operationOnStartLight, operationOnCompleteLight);
        CheckActiveSequence();

        _sequence.Append(_tweenDark);
        _sequence.Append(_tweenLight);
    }
    public Tween LightAnimationFade(Action operationOnStart, Action operationOnComplete, float duration = DURATION_ANIM_IN_SECONDS)
    {
        CheckActiveSequence();
        return _image.DOFade(0f, duration).OnStart(() => { operationOnStart(); }).OnComplete(() => { operationOnComplete(); });
    }
    public Tween DarkAnimationFade(Action operationOnStart, Action operationOnComplete/*, float duration = DURATION_ANIM_IN_SECONDS*/)
    {
        CheckActiveSequence();
        return _image.DOFade(1f, DURATION_ANIM_IN_SECONDS).OnStart(() => { operationOnStart(); }).OnComplete(() => { operationOnComplete(); });
    }
    public Tween AnimationFade(Image image, float value, float duration, Action operationOnComplete = null, bool independedTimeScale = true)
    {
        CheckActiveSequence();
        if (operationOnComplete == null)
        {
            return image.DOFade(value, duration).SetUpdate(independedTimeScale);
        }
        else
        {
            return image.DOFade(value, duration).OnComplete(() => { operationOnComplete(); }).SetUpdate(independedTimeScale);
        }
    }
    public Tween AnimationFade(Action operationOnStart, Action operationOnComplete, float duration, float value)
    {
        //CheckActiveSequence();
        return _image.DOFade(value, duration).OnStart(()=> { operationOnStart(); }).OnComplete(() => { operationOnComplete(); });
    }
    public void AnimationFade(CanvasGroup canvasGroup, float value, float duration, Action operationOnComplete = null)
    {
        if (operationOnComplete == null)
        {
            canvasGroup.DOFade(value, duration).SetUpdate(true);
        }
        else
        {
            canvasGroup.DOFade(value, duration).OnComplete(()=> { operationOnComplete(); }).SetUpdate(true);
        }
    }
    public void AnimationFadeTimeScaleDependent(CanvasGroup canvasGroup, float value, float duration, Action operationOnComplete = null)
    {
        if (operationOnComplete == null)
        {
            canvasGroup.DOFade(value, duration);
        }
        else
        {
            canvasGroup.DOFade(value, duration).OnComplete(() => { operationOnComplete(); });
        }
    }
    //public void AnimationEmergencePanel(BeamTransform move, CanvasGroup canvasGroup, float endValue/*, float duration*/)
    //{
    //    CreateSequence();
    //    _sequence.Append(move.DOLocalMoveY(endValue, 1f));
    //    _sequence.Insert(0f, canvasGroup.DOFade(0f, 1f));
    //}
    //public void AnimationDisappearancePanel(BeamTransform move, CanvasGroup canvasGroup, float endValue/*, float duration*/)
    //{
    //    CreateSequence();
    //    _sequence.Append(move.DOLocalMoveY(endValue, 1f));
    //    _sequence.Insert(0f, canvasGroup.DOFade(1f, 1f));
    //}
    private void CreateSequence()
    {
        _sequence = DOTween.Sequence();
    }
    private void CheckActiveSequence()
    {
        if (_sequence == null)
        {
            CreateSequence();
        }
        else
        {
            _sequence.Kill();
            CreateSequence();
        }
    }
}
