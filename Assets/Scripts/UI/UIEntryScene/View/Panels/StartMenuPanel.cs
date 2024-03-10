using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class StartMenuPanel : MonoBehaviour
{
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonSettings;
    [Space(20f)]
    [SerializeField] private SettingsPanel _settingsPanel;
    [Space(20f)]
    [SerializeField] private Image _darkBackground;
    public SettingsPanel PanelSettings => _settingsPanel;

    public Button ButtonPlay => _buttonPlay;
    public Button ButtonSettings => _buttonSettings;
    public Image DarkBackground => _darkBackground;
}
