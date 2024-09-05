using NaughtyAttributes;
using UnityEngine;

public class GlassRef : MonoBehaviour
{
    [SerializeField, BoxGroup("Glasses")] private Transform[] _glasses;
    
    [SerializeField, BoxGroup("Settings"), Range(1,100), HorizontalLine(color:EColor.Blue)] private int _strengthGlass;
    public Transform[] Glasses => _glasses;
    public int StrengthGlass => _strengthGlass;
}
