using NaughtyAttributes;
using UnityEngine;

public class RoofRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Roof")] private Transform _roofNormal;
    [SerializeField, BoxGroup("Roof")] private Transform _roofDamaged1;
    [SerializeField, BoxGroup("Roof")] private Transform _roofDamaged2;
    [SerializeField, BoxGroup("Roof")] private Transform _roofDamaged3;
    [SerializeField, BoxGroup("Roof")] private Transform _supportRoof;
    [SerializeField, BoxGroup("Settings"), Range(1,100), HorizontalLine(color:EColor.Blue)] private int _strengthRoof;

    public Transform RoofNormal => _roofNormal;
    public Transform RoofDamaged1 => _roofDamaged1;
    public Transform RoofDamaged2 => _roofDamaged2;
    public Transform RoofDamaged3 => _roofDamaged3;
    public Transform SupportRoof => _supportRoof;

    public int StrengthRoof => _strengthRoof;
}