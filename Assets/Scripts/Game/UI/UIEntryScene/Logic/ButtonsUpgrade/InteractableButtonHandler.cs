using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableButtonHandler
{
    private readonly float _alphaDisable;
    private readonly float _alphaEnable;
    private CanvasGroup _canvasGroupTextPrice;
    private Image _icon;
    private Button _button;
    public InteractableButtonHandler(Button button, Image icon, TextMeshProUGUI textPrice)
    {
        _canvasGroupTextPrice = textPrice.GetComponent<CanvasGroup>();
        _button = button;
        _icon = icon;
        _alphaDisable = _button.colors.disabledColor.g;
        _alphaEnable = 1f;
    }

    public void InteractableDisable()
    {
        _button.interactable = false;
        _canvasGroupTextPrice.alpha = _alphaDisable;
        _icon.color = GetColorWithAlpha(_icon.color);
    }

    public void InteractableEnable()
    {
        _button.interactable = true;
        _canvasGroupTextPrice.alpha = _alphaEnable;
        _icon.color = Color.white;
    }

    private Color GetColorWithAlpha(Color color)
    {
        return new Color(color.r, color.g, color.b, _alphaDisable);
    }
    
}