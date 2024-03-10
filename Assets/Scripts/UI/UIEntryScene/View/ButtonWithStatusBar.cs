using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithStatusBar : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _indicatorPosition;
    [SerializeField] private TextMeshProUGUI _textPrice;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite _indicatorSegmentOn;
    [SerializeField] private Sprite _indicatorSegmentOff;
    [SerializeField] private CanvasGroup _canvasGroup;
    public Button Button => _button;
    public Image IconImage => _iconImage;
    public Sprite IndicatorSegmentOn => _indicatorSegmentOn;
    public Sprite IndicatorSegmentOff => _indicatorSegmentOff;
    public CanvasGroup CanvasGroup => _canvasGroup;
    public RectTransform IndicatorPosition => _indicatorPosition;
    public TextMeshProUGUI TextPrice => _textPrice;
}
