using UnityEngine;

public class HotWheelRef : MonoBehaviour
{
    [SerializeField] private Transform _wheel1;
    [SerializeField] private Transform _wheel2;
    [SerializeField] private CircleCollider2D _wheel1Collider;
    [SerializeField] private CircleCollider2D _wheel2Collider;
    
    public Transform Wheel1 => _wheel1;
    public Transform Wheel2 => _wheel2;
    public CircleCollider2D Wheel1Collider => _wheel1Collider;
    public CircleCollider2D Wheel2Collider => _wheel2Collider;
}