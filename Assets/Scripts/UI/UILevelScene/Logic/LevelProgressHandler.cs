using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressHandler
{
    private Slider _levelProgress;
    private Slider _levelProgressLast;
    private LevelProgressCounter _levelProgressCounter;
    public LevelProgressHandler(ViewUILevel viewUILevel, LevelProgressCounter levelProgressCounter, ReactiveCommand disposeCommand)
    {
        _levelProgress = viewUILevel.LevelProgress;
        _levelProgressLast = viewUILevel.LevelLastProgress;
        _levelProgressLast.gameObject.SetActive(false);
        _levelProgressCounter = levelProgressCounter;
        _levelProgressCounter.OnProgressChanged += SetProgress;
        if (levelProgressCounter.ResultPreviosLevelForSlider != null)
        {
            SetLastProgress(levelProgressCounter.ResultPreviosLevelForSlider.LastSliderProgressValue);
        }
        disposeCommand.Subscribe(_ => { Dispose();});
    }

    private void Dispose()
    {
        _levelProgressCounter.OnProgressChanged -= SetProgress;
    }
    private void SetProgress(float value)
    {
        _levelProgress.value = value;
    }
    private void SetLastProgress(float value)
    {
        _levelProgressLast.gameObject.SetActive(true);
        _levelProgressLast.value = value;
    }
}
