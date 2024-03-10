using UnityEngine;

public class WheelCarValues : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _wheelRigidbody2D;
    [SerializeField] private Vector2 _anchor;
    public Rigidbody2D WheelRigidbody2D => _wheelRigidbody2D;
    public Vector2 Anchor => _anchor;
}
