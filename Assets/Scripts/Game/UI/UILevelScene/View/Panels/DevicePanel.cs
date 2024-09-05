using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevicePanel : MonoBehaviour
{
    [SerializeField] private RectTransform _transformArrowFuel;
    [SerializeField] private RectTransform _transformArrowSpeed;
    [SerializeField] private RectTransform _transformArrowAccelerator;
    [SerializeField] private TextMeshProUGUI _ammoIndicator;
    [SerializeField] private Image _indicatorFuel;
    [SerializeField] private Image indicatorFuelBooster;
    [SerializeField] private Color _colorActive;
    [SerializeField] private Color _colorDisactive;

    public RectTransform ArrowFuelTransform => _transformArrowFuel;
    public RectTransform ArrowSpeedTransform => _transformArrowSpeed;
    public RectTransform ArrowAcceleratorTransform => _transformArrowAccelerator;
    public TextMeshProUGUI AmmoIndicator => _ammoIndicator;
    public Image IndicatorFuel => _indicatorFuel;
    public Image IndicatorFuelBooster => indicatorFuelBooster;
    public Color ColorActive => _colorActive;
    public Color ColorDisactive => _colorDisactive;
}
