using UnityEngine;
using UnityEngine.UI;

public class BlinkIndicator
{
    private const float PERIOD_BLINKING = 0.5f;
    private const float START_BLINKING_VALUE = 0.15f;
    private const float END_BLINKING_VALUE = 0.001f;
    private float _timeIndicatorBlinking;
    private bool _toSwitchColorBlink;
    private Image _indicator;
    private Color _colorActive;
    private Color _colorDisactive;
    public BlinkIndicator(Image indicator, Color colorActive, Color colorDisactive)
    {
        _indicator = indicator;
        _colorActive = colorActive;
        _colorDisactive = colorDisactive;
        _toSwitchColorBlink = false;
        _timeIndicatorBlinking = PERIOD_BLINKING;
    }
    public void TryBlinking(float result, bool tankFullness)
    {
        if (tankFullness == true)
        {
            SetColorDisactive();
        }
        else
        {
            if (result > END_BLINKING_VALUE && result < START_BLINKING_VALUE)
            {
                BlinkingIndicatorFuel();
            }
            else
            {
                SetColorActive();
            }
        }
    }
    private void BlinkingIndicatorFuel()
    {
        _timeIndicatorBlinking -= Time.deltaTime;
        if (_timeIndicatorBlinking <= 0f)
        {
            _timeIndicatorBlinking = PERIOD_BLINKING;
            if (_toSwitchColorBlink == true)
            {
                _indicator.color = _colorDisactive;
                _toSwitchColorBlink = false;
            }
            else
            {
                _indicator.color = _colorActive;
                _toSwitchColorBlink = true;
            }
        }
    }

    private void SetColorActive()
    {
        _indicator.color = _colorActive;
    }
    private void SetColorDisactive()
    {
        _indicator.color = _colorDisactive;
    }
}