using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BoosterRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Booster")] private Transform[] _boosterParts;
    [SerializeField, BoxGroup("Settings"), Range(1,100), HorizontalLine(color:EColor.Blue)] private int _strengthBooster;
    public Transform[] BoosterParts => _boosterParts;
    public int StrengthBooster => _strengthBooster;
}
