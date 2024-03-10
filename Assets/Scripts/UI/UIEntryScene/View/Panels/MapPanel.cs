using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour
{
    [SerializeField] private Button _buttonGarage;
    [SerializeField] private Button _buttonBackToMenu;
    [SerializeField] private TextMeshProUGUI textDate;

    public Button ButtonGarage => _buttonGarage;
    public Button ButtonBackToStartMenu => _buttonBackToMenu;
    public TextMeshProUGUI TextDate => textDate;
}
