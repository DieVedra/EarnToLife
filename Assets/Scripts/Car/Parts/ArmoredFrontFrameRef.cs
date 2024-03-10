using NaughtyAttributes;
using UnityEngine;

public class ArmoredFrontFrameRef : MonoBehaviour
{
    [SerializeField, BoxGroup("ArmoredFront")] private Transform _armoredFrontNormal;
    [SerializeField, BoxGroup("ArmoredFront")] private Transform _armoredFrontDamaged;
 
    [SerializeField, BoxGroup("Settings"), Range(1,50)] private int _strengthArmoredFront;

    public Transform ArmoredFrontNormal => _armoredFrontNormal;
    public Transform ArmoredFrontDamaged => _armoredFrontDamaged;
    public int StrengthArmoredFront => _strengthArmoredFront;
}