using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelProgressCounter
{
    private Transform _startPoint;
    private Transform _endPoint;
    private Transform _carTransform;
    private SliderSectionValues _sliderSectionValues;
    private NotificationsProvider _notificationsProvider;
    private float _currentDistanceForDisplaySlider;
    private float _currentCarDistance;
    private bool InProgress = true;
    public ResultLevelForSlider ResultPreviosLevelForSlider { get;}
    public event Action<float> OnProgressChanged;
    public event Action OnGotPointDestination;
    public LevelProgressCounter(Transform startPoint, Transform endPoint, Transform car, NotificationsProvider notificationsProvider, SliderSectionValues sliderSectionValues, ResultLevelForSlider resultLevelForSlider)
    {
        _startPoint = startPoint;
        _endPoint = endPoint;
        _carTransform = car;
        _sliderSectionValues = sliderSectionValues;
        _notificationsProvider = notificationsProvider;
        ResultPreviosLevelForSlider = resultLevelForSlider;
    }
    public void CalculateProgress()
    {
        if (InProgress == true)
        {
            _currentCarDistance = Mathf.InverseLerp(_startPoint.position.x, _endPoint.position.x, _carTransform.position.x);
            _currentDistanceForDisplaySlider = Mathf.Lerp(_sliderSectionValues.SectionStartValue, _sliderSectionValues.SectionEndValue, _currentCarDistance);

            OnProgressChanged?.Invoke(_currentDistanceForDisplaySlider);
            if (_carTransform.position.x >= _endPoint.position.x)
            {
                OnGotPointDestination?.Invoke();
                _notificationsProvider.GotPointDestination();
                InProgress = false;
            }
        }
    }
    public float GetFinalDistance()
    {
        return _carTransform.position.x - _startPoint.position.x;
    }
    public float GetFinalDistanceToDisplayOnSliderInScorePanel()
    {
        return _currentCarDistance;
    }
    public ResultLevelForSlider GetCarDistanceForDisplaySlider()
    {
        return new ResultLevelForSlider(_currentDistanceForDisplaySlider);
    }
}
