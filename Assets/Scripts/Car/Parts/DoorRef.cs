using NaughtyAttributes;
using UnityEngine;

public class DoorRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Doors")] private Transform _doorNormal;
    [SerializeField, BoxGroup("Doors")] private Transform _doorDamaged1;
    [SerializeField, BoxGroup("Doors")] private Transform _doorDamaged2;
    [SerializeField, BoxGroup("Doors")] private HingeJoint2D _doorHingeJoint;
    [SerializeField, BoxGroup("Content")] private Transform _rearviewMirror;
    [SerializeField, BoxGroup("Settings"), Range(1,500), HorizontalLine(color:EColor.Blue)] private int _strengthDoors;
    [SerializeField, BoxGroup("Settings")] private Transform _pointEffect;

    public Transform DoorNormal => _doorNormal;
    public Transform DoorDamaged1 => _doorDamaged1;
    public Transform DoorDamaged2 => _doorDamaged2;
    public Transform PointEffect => _pointEffect;
    public Transform RearviewMirror => _rearviewMirror;
    public HingeJoint2D DoorHingeJoint => _doorHingeJoint;
    public int StrengthDoors => _strengthDoors;
}