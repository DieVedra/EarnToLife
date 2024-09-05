using System;
using NaughtyAttributes;
using UnityEngine;

public class BumperRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Bumpers")] private Transform _bumperNormal;
    [SerializeField, BoxGroup("Bumpers")] private Transform _bumperDamaged;
    [SerializeField, BoxGroup("Settings"), Range(1,500), HorizontalLine(color:EColor.Blue)] private int _strengthBumper;
    public Transform BumperNormal => _bumperNormal;
    public Transform BumperDamaged => _bumperDamaged;
    public int StrengthBumper => _strengthBumper;
}