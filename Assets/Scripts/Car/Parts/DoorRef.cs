using NaughtyAttributes;
using UnityEngine;

public class DoorRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Doors")] private Transform _doorNormal;
    [SerializeField, BoxGroup("Doors")] private Transform _doorDamaged1;
    [SerializeField, BoxGroup("Doors")] private Transform _doorDamaged2;
    [SerializeField, BoxGroup("Doors")] private HingeJoint2D _doorHingeJoint;
    [SerializeField, BoxGroup("Settings"), Range(1,100), HorizontalLine(color:EColor.Blue)] private int _strengthDoors;

    public Transform DoorNormal => _doorNormal;
    public Transform DoorDamaged1 => _doorDamaged1;
    public Transform DoorDamaged2 => _doorDamaged2;
    public HingeJoint2D DoorHingeJoint => _doorHingeJoint;
    public int StrengthDoors => _strengthDoors;
}