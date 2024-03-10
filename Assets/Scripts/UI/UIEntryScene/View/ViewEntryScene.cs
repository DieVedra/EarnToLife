using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewEntryScene : MonoBehaviour
{
    [SerializeField] private StartMenuPanel _startMenuPanel;
    [SerializeField] private MapPanel mapPanel;
    [SerializeField] private GaragePanel _garagePanel;
    [Space(20f)]
    [SerializeField] private Image _darkBackground;

    public StartMenuPanel PanelStartMenu => _startMenuPanel;
    public MapPanel PanelMap => mapPanel;
    public GaragePanel PanelGarage => _garagePanel;
    public Image DarkBackground => _darkBackground;
}
