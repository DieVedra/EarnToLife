using NaughtyAttributes;
using UnityEngine;

public class BottomRef : MonoBehaviour
{
    [SerializeField] private Transform _backCar;
    [SerializeField] private Transform _armoredBottom;
    [SerializeField] private Transform _exhaust;
    [SerializeField] private WheelCarValues _invisibleWheel;
    
    [SerializeField, BoxGroup("Settings"), Range(1,5000)] private int _strengthArmoredBottom;
    [SerializeField, BoxGroup("Settings"), Range(1,5000)] private int _strengthBottom;

    public Transform BackCar => _backCar;
    public Transform ArmoredBottom => _armoredBottom;
    public Transform Exhaust => _exhaust;
    public WheelCarValues InvisibleWheel => _invisibleWheel;

    public int StrengthBottom => _strengthBottom;
    public int StrengthArmoredBottom => _strengthArmoredBottom;

}