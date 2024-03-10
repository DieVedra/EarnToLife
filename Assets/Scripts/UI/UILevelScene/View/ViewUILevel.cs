using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class ViewUILevel : MonoBehaviour
{
    [SerializeField] private PanelPause _panelPause;
    [SerializeField] private PanelScore _panelScore;
    [SerializeField] private DevicePanel _devicePanel;
    [SerializeField, HorizontalLine(color:EColor.White)] private Image _background;
    [SerializeField] private Image _frameBackground;
    [SerializeField, HorizontalLine(color:EColor.White)] private Slider _levelProgress;
    [SerializeField] private Slider _levelLastProgress;
    [SerializeField, HorizontalLine(color:EColor.White)] private Button _buttonPause;
    [SerializeField] private CustomButton _buttonGas;
    [SerializeField] private CustomButton _buttonBoost;
    [SerializeField] private CustomButton _buttonStop;
    [SerializeField] private CustomButton _buttonRotateClockwise;
    [SerializeField] private CustomButton _buttonRotateСounterСlockwise;
    [SerializeField] private CustomButton _buttonFire;
    [SerializeField, HorizontalLine(color:EColor.White)] private Button _buttonCrosshair;
    [SerializeField, HorizontalLine(color:EColor.White)] private TextMeshProUGUI _notificationsText;

    public PanelPause PanelPause => _panelPause;
    public PanelScore PanelScore => _panelScore;
    public DevicePanel DevicePanel => _devicePanel;
    public Image Background => _background;
    public Image FrameBackground => _frameBackground;
    public Slider LevelProgress => _levelProgress;
    public Slider LevelLastProgress => _levelLastProgress;
    public Button ButtonPause => _buttonPause;
    public Button ButtonCrosshair => _buttonCrosshair;
    public CustomButton ButtonGas => _buttonGas; 
    public CustomButton ButtonBoost => _buttonBoost;
    public CustomButton ButtonStop => _buttonStop;
    public CustomButton ButtonRotateClockwise => _buttonRotateClockwise;
    public CustomButton ButtonRotateCounterClockwise => _buttonRotateСounterСlockwise;
    public CustomButton ButtonFire => _buttonFire;
    public TextMeshProUGUI NotificationsText => _notificationsText;
    public ReactiveCommand DisposeCommand { get; private set; } = new ReactiveCommand();
    public IReadOnlyList<Button> GetUIControlButtons()
    {
        return new Button[]
        {
            ButtonGas, ButtonBoost, ButtonStop, ButtonRotateClockwise, ButtonRotateCounterClockwise, ButtonFire
        };
    }

    private void OnDisable()
    {
        DisposeCommand.Execute();
        DisposeCommand.Dispose();
    }
}
