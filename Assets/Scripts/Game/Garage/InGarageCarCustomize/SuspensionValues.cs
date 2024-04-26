using UnityEngine;

public class SuspensionValues : MonoBehaviour
{
    [SerializeField] private Transform _spring;
    [SerializeField] private Transform _insideCilinder;
    [SerializeField] private float _suspensionStiffness;
    [SerializeField] private float _suspensionYScaleValueDefault;
    public Transform Spring => _spring;
    public Transform InsideCylinder => _insideCilinder;
    public float SuspensionStiffness => _suspensionStiffness;
    public float SuspensionYScaleValueDefault => _suspensionYScaleValueDefault;
}