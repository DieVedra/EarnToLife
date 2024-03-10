using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Button _buttonMusic;
    [SerializeField] private Button _buttonSound;
    [SerializeField] private Button _buttonExit;
    [SerializeField] private Image _frameBackground;
    [SerializeField] private TextMeshProUGUI _musicTextStatus;
    [SerializeField] private TextMeshProUGUI _soundTextStatus;
    public Button ButtonMusic => _buttonMusic;
    public Button ButtonSound => _buttonSound;
    public Button ButtonExit => _buttonExit;
    public Image FrameBackground => _frameBackground;
    public TextMeshProUGUI MusicTextStatus => _musicTextStatus;
    public TextMeshProUGUI SoundTextStatus => _soundTextStatus;

}
