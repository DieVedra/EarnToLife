using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GunRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Gun")] private Transform[] _gunParts;
    [SerializeField, BoxGroup("Settings"), Range(1,100), HorizontalLine(color:EColor.Blue)] private int _strengthGun;
    public Transform[] GunParts => _gunParts;
    public int StrengthGun => _strengthGun;
}
