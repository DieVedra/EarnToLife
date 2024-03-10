using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BackWingRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Wings")] private Transform _wingNormal;
    [SerializeField, BoxGroup("Wings")] private Transform _wingDamaged1;
    [SerializeField, BoxGroup("Wings")] private Transform _wingDamaged2;
    [SerializeField, BoxGroup("Wings")] private Transform[] _wingContent;
    [SerializeField, BoxGroup("Settings"), Range(1,100), HorizontalLine(color:EColor.Blue)] private int _strengthWing;
    public Transform WingNormal => _wingNormal;
    public Transform WingDamaged1 => _wingDamaged1;
    public Transform WingDamaged2 => _wingDamaged2;
    public IReadOnlyList<Transform> WingContent => _wingContent;
    public int StrengthWing => _strengthWing;
}