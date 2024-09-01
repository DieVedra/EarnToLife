using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton
{
    private readonly float _alphaNotSufficientAmountMoney = 0.4f;
    private readonly Color _colorSufficientAmountMoney;
    private readonly Color _colorNotSufficientAmountMoney;
    // private readonly string _name;
    // private readonly string _description;
    private Button _button;
    private ConfirmationUpgrade _confirmationUpgrade;
    private TextMeshProUGUI _textPrice;
    private ProgressIndicator _progressIndicator;
    private InteractableButtonHandler _interactableButtonHandler;
    private AudioHandlerUI _audioHandlerUI;
    private Image _iconImage;
    private ButtonData _buttonData;
    private Func<bool> _upgradeOperation;
    private bool _isClicked;
    public UpgradeButton(ButtonWithStatusBar buttonWithStatusBar, ConfirmationUpgrade confirmationUpgrade, AudioHandlerUI audioHandlerUI, Func<bool> operationUpgrade, Image segmentIconPrefab, Spawner spawner, string name, string description)
    {
        _audioHandlerUI = audioHandlerUI;
        // _name = name;
        // _description = description;
        _textPrice = buttonWithStatusBar.TextPrice;
        _button = buttonWithStatusBar.Button;
        _iconImage = buttonWithStatusBar.IconImage;
        _confirmationUpgrade = confirmationUpgrade;
        _upgradeOperation = operationUpgrade;
        _buttonData = new ButtonData(_iconImage, name, description);
        _progressIndicator = new ProgressIndicator(buttonWithStatusBar.IndicatorPosition, segmentIconPrefab, spawner, buttonWithStatusBar.IndicatorSegmentOn, buttonWithStatusBar.IndicatorSegmentOff);
        _interactableButtonHandler = new InteractableButtonHandler(buttonWithStatusBar.Button, buttonWithStatusBar.IconImage, buttonWithStatusBar.TextPrice);
        _colorSufficientAmountMoney = _textPrice.color;
        _colorNotSufficientAmountMoney = GetColorNotSufficientAmount(_textPrice.color);
        _isClicked = false;
    }
    private void Click()
    {
        EnterClickState();
        _confirmationUpgrade.ActivatePanel(_upgradeOperation, _progressIndicator, _buttonData);
        _confirmationUpgrade.SetColorAvailabilityBuy(_textPrice.color.a);
        _confirmationUpgrade.SetPrice(_textPrice.text);
    }
    private void EnterClickState()
    {
        _isClicked = true;
        _confirmationUpgrade.OnDeactivatePanel += ExitClickState;
        UnsubscribeClickButton();
        _interactableButtonHandler.InteractableDisable();
    }

    private void ExitClickState()
    {
        _isClicked = false;
        SubscribeClickButton();
        _interactableButtonHandler.InteractableEnable();
        _confirmationUpgrade.OnDeactivatePanel -= ExitClickState;
    }
    public void Activate()
    {
        SubscribeClickButton();
    }
    public void Deactivate()
    {
        UnsubscribeClickButton();
    }
    public void ResetIndicator()
    {
        _progressIndicator.Reset();
    }
    public void CreateIndicatorBar(int count, int countActive)
    {
        _progressIndicator.CreateIcon(count, countActive);
    }
    public void SetPrice(string price)
    {
        _textPrice.text = price;
        if (_isClicked == true)
        {
            _confirmationUpgrade.SetPrice(price);
        }
    }
    public void SetIcon(Sprite sprite)
    {
        _iconImage.sprite = sprite;
    }
    public void ResetIcon()
    {
        _iconImage.sprite = null;
    }
    public void SetColorSufficientAmountMoneyToBuy()
    {
        SetColorText(_colorSufficientAmountMoney);
    }
    public void SetColorNotSufficientAmountMoneyToBuy()
    {
        SetColorText(_colorNotSufficientAmountMoney);
    }
    private void SetColorText(Color color)
    {
        if (_textPrice.color != color)
        {
            _textPrice.color = color;
        }
        if (_isClicked == true)
        {
            _confirmationUpgrade.SetColorAvailabilityBuy(color.a);
        }
    }
    private Color GetColorNotSufficientAmount(Color color)
    {
        return new Color(color.r, color.g, color.b, _alphaNotSufficientAmountMoney);
    }
    private void SubscribeClickButton()
    {
        _button.onClick.AddListener(() =>
        {
            Click();
            _audioHandlerUI.PlayClick();
        });
    }
    private void UnsubscribeClickButton()
    {
        _button.onClick.RemoveAllListeners();
    }
}
