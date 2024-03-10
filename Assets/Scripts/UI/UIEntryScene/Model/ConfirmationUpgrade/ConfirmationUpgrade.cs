using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationUpgrade
{
    private const string MAX = "Max";
    private RectTransform _rectTransformDescription;
    private Image _iconDescription;
    private Button _buttonOK;
    private Button _buttonCancel;
    private TextMeshProUGUI _price;
    private TextMeshProUGUI _description;
    private TextMeshProUGUI _name;
    private ProgressIndicator _progressIndicator;
    private ProgressIndicator _progressIndicatorCurrentButton;
    private AnimationPanelConfirmationUpgrade _animationPanelConfirmationUpgrade;
    private AudioHandlerUI _audioHandlerUI;
    private Func<bool> _upgradeOperation;
    public event Action OnDeactivatePanel;
    public event Action OnUpgradePricesUpdate;

    public ConfirmationUpgrade(PanelConfirmationUpgrade panelConfirmationUpgrade, Spawner spawner, AudioHandlerUI audioHandlerUI, Image iconPrefab, Sprite iconOn, Sprite iconOff)
    {
        _audioHandlerUI = audioHandlerUI;
        _buttonOK = panelConfirmationUpgrade.ButtonOK;
        _buttonCancel = panelConfirmationUpgrade.ButtonCancel;
        _price = panelConfirmationUpgrade.PriceText;
        _name = panelConfirmationUpgrade.Name;
        _description = panelConfirmationUpgrade.DescriptionText;
        _iconDescription = panelConfirmationUpgrade.IconDescription;
        _rectTransformDescription = panelConfirmationUpgrade.IconDescription.GetComponent<RectTransform>();
        _animationPanelConfirmationUpgrade = new AnimationPanelConfirmationUpgrade(panelConfirmationUpgrade.RectTransformPanel, panelConfirmationUpgrade.FrameBackground);
        _progressIndicator = new ProgressIndicator(panelConfirmationUpgrade.IndicatorRectTransform, iconPrefab, spawner, iconOn, iconOff);
        DeactivatePanel();
    }
    public void ActivatePanel(Func<bool> operationUpgrade, ProgressIndicator progressIndicatorFromButton, ButtonData buttonData)
    {
        _upgradeOperation = operationUpgrade;
        _progressIndicatorCurrentButton = progressIndicatorFromButton;
        SetDescriptionAndIcon(buttonData);
        SubscribeButtons();
        CreateIndicatorBar(progressIndicatorFromButton);
        _animationPanelConfirmationUpgrade.UnhidePanel();
    }

    private async void DeactivatePanel()
    {
        UnsubscribeButtons();
        OnDeactivatePanel?.Invoke();
        await _animationPanelConfirmationUpgrade.HidePanel();
        DestroyIndicatorBar();
    }

    private void TryBuyUpgrade()
    {
        if (_upgradeOperation?.Invoke() == true)
        {
            _audioHandlerUI.PlayBuySuccess();
            _progressIndicator.Evaluate();
            _progressIndicatorCurrentButton.Evaluate();
            OnUpgradePricesUpdate?.Invoke();
        }
        else
        {
            _audioHandlerUI.PlayBuyFail();
            //open shop
        }
    }
    private void CreateIndicatorBar(ProgressIndicator progressIndicatorFromButton)
    {
        _progressIndicator.CreateIcon(progressIndicatorFromButton.CountSegments, progressIndicatorFromButton.CountActiveSegments);
    }
    private void DestroyIndicatorBar()
    {
        _progressIndicator.Reset();
    }
    private void SubscribeButtons()
    {
        _buttonOK.onClick.AddListener(TryBuyUpgrade);
        _buttonCancel.onClick.AddListener(() =>
        {
            _audioHandlerUI.PlayClick();
            DeactivatePanel();
        });
    }
    private void UnsubscribeButtons()
    {
        _buttonOK.onClick.RemoveAllListeners();
        _buttonCancel.onClick.RemoveAllListeners();
    }
    public void SetColorAvailabilityBuy(float alpha)
    {
        _price.color = new Color(_price.color.r, _price.color.g, _price.color.b, alpha);
    }
    public void SetPrice(string count)
    {
        _price.text = $"{count}";
        if (count == MAX)
        {
            OkButtonInteractableDisable();
        }
        else
        {
            OkButtonInteractableEnable();
        }
    }

    private void SetDescriptionAndIcon(ButtonData buttonData)
    {
        _rectTransformDescription.sizeDelta = buttonData.Icon.rectTransform.sizeDelta;
        _rectTransformDescription.rotation = buttonData.Icon.rectTransform.rotation;
        _iconDescription.sprite = buttonData.Icon.sprite;
        _name.text = buttonData.Name;
        _description.text = buttonData.Description;
    }

    private void OkButtonInteractableEnable()
    {
        _buttonOK.interactable = true;
    }
    private void OkButtonInteractableDisable()
    {
        _buttonOK.interactable = false;
    }
}
