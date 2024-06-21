using UnityEngine;

public class HotWheelRef : MonoBehaviour
{
    [SerializeField] private Transform _wheel1;
    [SerializeField] private Transform _wheel2;
    [SerializeField] private CircleCollider2D _wheel1Collider;
    [SerializeField] private CircleCollider2D _wheel2Collider;
    [SerializeField] private Rigidbody2D _rigidbody2DWheel1;
    [SerializeField] private Rigidbody2D _rigidbody2DWheel2;
    
    public Transform Wheel1 => _wheel1;
    public Transform Wheel2 => _wheel2;
    public CircleCollider2D Wheel1Collider => _wheel1Collider;
    public CircleCollider2D Wheel2Collider => _wheel2Collider;
    public Rigidbody2D Rigidbody2DWheel1 => _rigidbody2DWheel1;
    public Rigidbody2D Rigidbody2DWheel2 => _rigidbody2DWheel2;
}