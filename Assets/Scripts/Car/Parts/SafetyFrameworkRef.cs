using NaughtyAttributes;
using UnityEngine;

public class SafetyFrameworkRef : MonoBehaviour
{
    [SerializeField] private Transform _support1;
    [SerializeField] private Transform _support2;
    [SerializeField, BoxGroup("Settings"), Range(1,1000), HorizontalLine(color:EColor.Blue)] private int _strengthSafetyFramework;
    
    public Transform Support1 => _support1;
    public Transform Support2 => _support2;
    public int StrengthSafetyFramework => _strengthSafetyFramework;

}