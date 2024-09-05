using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BackWingRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Wings")] private Transform _wingNormal;
    [SerializeField, BoxGroup("Wings")] private Transform _wingDamaged1;
    [SerializeField, BoxGroup("Wings")] private Transform _wingDamaged2;
    [SerializeField, BoxGroup("Content")] private Transform _trunkCover2;
    [SerializeField, BoxGroup("Content")] private Transform[] _wingContent;
    [SerializeField, BoxGroup("Content")] private Transform[] _trunkCovers;
    [SerializeField, BoxGroup("Settings"), Range(1,1000), HorizontalLine(color:EColor.Blue)] private int _strengthWing;
    public Transform WingNormal => _wingNormal;
    public Transform WingDamaged1 => _wingDamaged1;
    public Transform WingDamaged2 => _wingDamaged2;
    public Transform TrunkCover2 => _trunkCover2;
    public IReadOnlyList<Transform> WingContent => _wingContent;
    public IReadOnlyList<Transform> TrunkCovers => _trunkCovers;
    public int StrengthWing => _strengthWing;
}