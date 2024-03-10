using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelScore : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button _buttonGarage;
    [SerializeField] private Slider _levelProgressResults;
    [SerializeField] private Slider _levelProgressLastResults;
    [SerializeField] private TextMeshProUGUI _textDay;
    [SerializeField] private TextMeshProUGUI _textReason;
    [SerializeField] private TextMeshProUGUI _textDistance;
    [SerializeField] private TextMeshProUGUI _textKills;
    [SerializeField] private TextMeshProUGUI _textCash;
    public RectTransform RectTransform => _rectTransform;
    public Button ButtonGarage => _buttonGarage;
    public Slider LevelProgressResults => _levelProgressResults;
    public Slider LevelProgressLastResults => _levelProgressLastResults;
    public TextMeshProUGUI TextDay => _textDay;
    public TextMeshProUGUI TextReason => _textReason;
    public TextMeshProUGUI TextDistance => _textDistance;
    public TextMeshProUGUI TextKills => _textKills;
    public TextMeshProUGUI TextCash => _textCash;
}
