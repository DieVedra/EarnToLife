using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfirmationUpgrade : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransformPanel;
    [SerializeField] private RectTransform _indicatorRectTransform;
    [SerializeField] private Image _frameBackground;
    [SerializeField] private Image _iconDescription;
    [SerializeField] private Button _buttonOK;
    [SerializeField] private Button _buttonCancel;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _name;

    public RectTransform RectTransformPanel => _rectTransformPanel;
    public RectTransform IndicatorRectTransform => _indicatorRectTransform;
    public Image FrameBackground => _frameBackground;
    public Image IconDescription => _iconDescription;
    public Button ButtonOK => _buttonOK;
    public Button ButtonCancel => _buttonCancel;
    public TextMeshProUGUI PriceText => _price;
    public TextMeshProUGUI DescriptionText => _description;
    public TextMeshProUGUI Name => _name;
}
