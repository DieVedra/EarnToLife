using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 51)]
public class GameData : ScriptableObject
{
    public PanelsInMenu StartPanelInMenu { get; set; }
    public CarControlMethod CarControlMethod { get; set; }
    
    public bool SaveOn { get; set; }
}
