using UnityEngine;

public class HotWheelRef : MonoBehaviour
{
    [SerializeField] private Transform _wheel1;
    [SerializeField] private Transform _wheel2;
    
    public Transform Wheel1 => _wheel1;
    public Transform Wheel2 => _wheel2;
}