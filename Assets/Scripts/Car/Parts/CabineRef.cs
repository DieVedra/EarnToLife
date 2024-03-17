using UnityEngine;

public class CabineRef : MonoBehaviour
{
    [SerializeField] private Transform _helmet;
    [SerializeField] private Transform _headrest;
    
    
    
    public Transform Helmet => _helmet;
    public Transform Headrest => _headrest;
}