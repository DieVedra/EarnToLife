using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressIndicator
{
    private const int MAX_COUNT_SEGMENTS = 10;
    private const int MIN_COUNT_SEGMENTS = 0;
    private const float MULTIPLY_SIZE = 12.86f;
    private readonly Vector2 _fullSizeSection;
    private readonly Vector2 _sizeSegment;
    private readonly float _indentationXBetweenSegments;
    private float _posXSegment;
    private Sprite _iconOn;
    private Sprite _iconOff;
    private Image _segmentPrefab;
    private Spawner _spawner;
    private RectTransform _rectTransform;
    private List<Image> _segments;
    public int CountSegments { get; private set; }
    public int CountActiveSegments { get; private set; }
    public ProgressIndicator(RectTransform rectTransform, Image segmentPrefab, Spawner spawner, Sprite iconOn, Sprite iconOff)
    {
        _rectTransform = rectTransform;
        _fullSizeSection = _rectTransform.sizeDelta;
        _segmentPrefab = segmentPrefab;
        _spawner = spawner;
        _segments = new List<Image>(MAX_COUNT_SEGMENTS);
        _iconOn = iconOn;
        _iconOff = iconOff;
        _sizeSegment = CalculateSizeSegment();
        _indentationXBetweenSegments = CalculateIndentationBetweenSegments();
    }
    public void Evaluate()
    {    
        if (CountSegments > CountActiveSegments)
        {
            CountActiveSegments++;
            SetActiveSegments();
        }
    }
    public void Reset()
    {
        for (int i = 0; i < _segments.Count; i++)
        {
            _spawner.KillObject(_segments[i].gameObject);
        }
        _segments.Clear();
    }

    public void CreateIcon(int countSegments, int countActiveSegments)
    {
        if (CheckRangeSegmentsCount(countSegments) == true)
        {
            CountSegments = countSegments;
            CountActiveSegments = countActiveSegments;
            BuildSegments();
            SetActiveSegments();
        }
        else
        {
            Debug.LogError("Going beyond the range of the number of segments");
        }
    }
    private void BuildSegments()
    {
        for (int i = 0; i < CountSegments; i++)
        {
            _segments.Add(CreateSegment());
            SetSpriteOff(_segments[i]);
            if (i == 0)
            {
                _posXSegment = CalculatePositionFirstSegment();
            }
            else
            {
                _posXSegment = _posXSegment + _indentationXBetweenSegments;
            }
            SetSizeSegment(_segments[i], _sizeSegment);
            SetPositionSegment(_segments[i], new Vector2(_posXSegment, 0f));
        }
    }
    private bool CheckParity()
    {
        if (CountSegments % 2 == 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool CheckRangeSegmentsCount(int countSegments)
    {
        if (countSegments <= MIN_COUNT_SEGMENTS || countSegments > MAX_COUNT_SEGMENTS)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void SetActiveSegments()
    {
        if (CountActiveSegments != 0)
        {
            for (int i = 0; i < CountActiveSegments; i++)
            {
                SetSpriteOn(_segments[i]);
            }
        }
    }
    private void SetSpriteOn(Image frame)
    {
        frame.sprite = _iconOn;
    }
    private void SetSpriteOff(Image frame)
    {
        frame.sprite = _iconOff;
    }
    private Image CreateSegment()
    {
        return _spawner.Spawn(_segmentPrefab, _rectTransform, _rectTransform);
    }

    private void SetPositionSegment(Image segment ,Vector2 position)
    {
        segment.rectTransform.localPosition = position;
    }

    private void SetSizeSegment(Image segment, Vector2 size)
    {
        segment.rectTransform.sizeDelta = size;
    }
    private Vector2 CalculateSizeSegment()
    {
        float newWidth = _fullSizeSection.x / MULTIPLY_SIZE;
        return new Vector2(newWidth, _fullSizeSection.y);
    }
    private float CalculatePositionFirstSegment()
    {
        float halfCountSegments = CountSegments * 0.5f;
        float value;
        float indent;
        if (CheckParity() == true)
        {
            value = _indentationXBetweenSegments * halfCountSegments;
            indent = value - (_indentationXBetweenSegments * 0.5f);
        }
        else
        {
            indent = (_indentationXBetweenSegments * ((int) halfCountSegments));
        }
        indent = indent * -1f;
        return indent;
    }

    private float CalculateIndentationBetweenSegments()
    {
        float indentation = (_fullSizeSection.x - _sizeSegment.x) / (MAX_COUNT_SEGMENTS - 1);
        indentation = (float)Math.Round(indentation, 1);
        return indentation;
    }
}
