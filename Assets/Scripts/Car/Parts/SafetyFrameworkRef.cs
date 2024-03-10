using UnityEngine;

public class SafetyFrameworkRef : MonoBehaviour
{
    [SerializeField] private Transform _support1;
    [SerializeField] private Transform _support2;
    [SerializeField] private int _strengthSafetyFramework;
    
    public Transform Support1 => _support1;
    public Transform Support2 => _support2;
    public int StrengthSafetyFramework => _strengthSafetyFramework;

}