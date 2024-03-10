using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FrontWingRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Wings")] private Transform _wingNormal;
    [SerializeField, BoxGroup("Wings")] private Transform _wingDamaged1;
    [SerializeField, BoxGroup("Wings")] private Transform _wingDamaged2;
    [SerializeField, BoxGroup("FrontBottom")] private Transform _bottomNormal;
    [SerializeField, BoxGroup("FrontBottom")] private Transform _bottomDamaged;
    [SerializeField, BoxGroup("Content")] private Transform _lighterWingNormal;
    [SerializeField, BoxGroup("Content")] private Transform _wingFrontDamaged2TrunkCover;
    [SerializeField, BoxGroup("Settings"), Range(1,500), HorizontalLine(color:EColor.Blue)] private int _strengthWing;
    public Transform WingNormal => _wingNormal;
    public Transform WingDamaged1 => _wingDamaged1;
    public Transform WingDamaged2 => _wingDamaged2;
    public Transform BottomNormal => _bottomNormal;
    public Transform BottomDamaged => _bottomDamaged;
    public Transform LighterWingNormal => _lighterWingNormal;
    public Transform WingFrontDamaged2TrunkCover => _wingFrontDamaged2TrunkCover;
    public int StrengthWing => _strengthWing;
}
