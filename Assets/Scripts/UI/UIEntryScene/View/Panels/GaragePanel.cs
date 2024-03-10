using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaragePanel : MonoBehaviour
{
    [SerializeField] private Button _buttonGO;
    [SerializeField] private Button _buttonMap;
    [SerializeField] private Button _buttonSlideRight;
    [SerializeField] private Button _buttonSlideLeft;

    [SerializeField] private ButtonWithStatusBar _buttonEngineUpgrade;
    [SerializeField] private ButtonWithStatusBar _buttonGearboxUpgrade;
    [SerializeField] private ButtonWithStatusBar _buttonWheelsUpgrade;
    [SerializeField] private ButtonWithStatusBar _buttonGunUpgrade;
    [SerializeField] private ButtonWithStatusBar _buttonCorpusUpgrade;
    [SerializeField] private ButtonWithStatusBar _buttonBoosterUpgrade;
    [SerializeField] private ButtonWithStatusBar _buttonFueltankUpgrade;
    // [SerializeField] private CanvasGroup _canvasGroupButtonsUpgrade;
    [SerializeField] private RectTransform _parentRectTransformButtonsUpgrade;

    [Space (20)]
    [SerializeField] private TextMeshProUGUI _cash;

    [SerializeField] private Image _imageLock;
    [SerializeField] private PanelConfirmationUpgrade panelConfirmationUpgrade;

    // [SerializeField] private Color _colorTextLackOfMoney;
    // [SerializeField] private Color _colorTextEnoughMoney;
    public Button ButtonGO => _buttonGO;
    public Button ButtonBackToMap => _buttonMap;
    public Button ButtonSlideRight => _buttonSlideRight;
    public Button ButtonSlideLeft => _buttonSlideLeft;
    public ButtonWithStatusBar ButtonEngineUpgrade => _buttonEngineUpgrade;
    public ButtonWithStatusBar ButtonGearboxUpgrade => _buttonGearboxUpgrade;
    public ButtonWithStatusBar ButtonWheelsUpgrade => _buttonWheelsUpgrade;
    public ButtonWithStatusBar ButtonGunUpgrade => _buttonGunUpgrade;
    public ButtonWithStatusBar ButtonCorpusUpgrade => _buttonCorpusUpgrade;
    public ButtonWithStatusBar ButtonBoosterUpgrade => _buttonBoosterUpgrade;
    public ButtonWithStatusBar ButtonFueltankUpgrade => _buttonFueltankUpgrade;
    public RectTransform ParentRectTransformButtonsUpgrade => _parentRectTransformButtonsUpgrade;

    public TextMeshProUGUI Cash => _cash;
    public Image Lock => _imageLock;
    public PanelConfirmationUpgrade PanelConfirmationUpgrade => panelConfirmationUpgrade;

    // public void a()
    // {
    //     _buttonSlideLeft.O
    // }
    // public Color ColorTextLackOfMoney => _colorTextLackOfMoney;
    // public Color ColorTextEnoughMoney => _colorTextEnoughMoney;
}
