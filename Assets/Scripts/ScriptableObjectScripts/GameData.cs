using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 51)]
public class GameData : ScriptableObject
{
    public PanelsInMenu StartPanelInMenu;
    public ScreenOrientation ScreenOrientation;
    [SerializeField] private CarControlMethod _carControlMethod;
    [SerializeField] private bool _saveOn;
    [SerializeField, HorizontalLine(color: EColor.Gray), BoxGroup("Cash Wallet")] private int _cash;

    public bool SaveOn => _saveOn;
    public int Cash => _cash;

    public CarControlMethod GetCarControlMethod()
    {
        if (Application.isEditor == true)
        {
            return  _carControlMethod;
        }
        else if (Application.isMobilePlatform == true)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            _carControlMethod = CarControlMethod.SensorDisplayMethod;
            return _carControlMethod;
        }
        else
        {
            _carControlMethod = CarControlMethod.KeyboardMethod;

            return _carControlMethod;
        }
    }
}
