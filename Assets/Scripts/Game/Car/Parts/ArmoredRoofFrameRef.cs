using NaughtyAttributes;
using UnityEngine;

public class ArmoredRoofFrameRef : MonoBehaviour
{
    [SerializeField, BoxGroup("ArmoredRoof")] private Transform _frameNormal;
    [SerializeField, BoxGroup("ArmoredRoof")] private Transform _frameDamaged;
 
    [SerializeField, BoxGroup("Settings"), Range(1,1000)] private int _strengthArmoredRoof;
    
    public Transform FrameNormal => _frameNormal;
    public Transform FrameDamaged => _frameDamaged;
    public int StrengthArmoredRoof => _strengthArmoredRoof;
}