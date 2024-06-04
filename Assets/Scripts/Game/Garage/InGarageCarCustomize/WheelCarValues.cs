using UnityEngine;

public class WheelCarValues : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _wheelRigidbody2D;
    [SerializeField] private Vector2 _anchor;
    [SerializeField] private ParticleSystem _smokeWheelParticleSystem;
    [SerializeField] private ParticleSystem _dirtWheelParticleSystem;
    [SerializeField] private ParticleSystem _bloodWheelParticleSystem;
    public Rigidbody2D WheelRigidbody2D => _wheelRigidbody2D;
    public ParticleSystem SmokeWheelParticleSystem => _smokeWheelParticleSystem;
    public ParticleSystem DirtWheelParticleSystem => _dirtWheelParticleSystem;
    public ParticleSystem BloodWheelParticleSystem => _bloodWheelParticleSystem;
    public Vector2 Anchor => _anchor;
}
