using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelPause : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField, HorizontalLine(color:EColor.White)] private Button _buttonClose;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonGarage;
    [SerializeField, HorizontalLine(color:EColor.Blue)] private Button _buttonMusic;
    [SerializeField] private TextMeshProUGUI textOnOffButtonMusic;
    [SerializeField, HorizontalLine(color:EColor.Blue)] private Button _buttonSound;
    [SerializeField] private TextMeshProUGUI textOnOffButtonSound;

    public RectTransform RectTransform => _rectTransform;
    public Button ButtonClose => _buttonClose;
    public Button ButtonRestart => _buttonRestart;
    public Button ButtonGarage => _buttonGarage;
    public Button ButtonMusic => _buttonMusic;
    public Button ButtonSound => _buttonSound;
    public TextMeshProUGUI TextOnOffButtonMusic => textOnOffButtonMusic;
    public TextMeshProUGUI TextOnOffButtonSound => textOnOffButtonSound;
}
