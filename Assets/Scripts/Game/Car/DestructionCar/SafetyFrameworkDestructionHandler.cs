using UnityEngine;

public class SafetyFrameworkDestructionHandler
{
    private readonly Vector3 _scaleSupport1SafetyFrameworkAfterHit = new Vector3(1f, 0.7f,1f);
    private readonly Vector3 _scaleSupport2SafetyFrameworkAfterHit = new Vector3(1f, 0.62f,1f);
    private readonly int _debrisLayer;
    private readonly DebrisParent _debrisParent;
    private readonly SafetyFrameworkRef _safetyFrameworkRef;
    private bool _isBroken;
    
    public SafetyFrameworkDestructionHandler(SafetyFrameworkRef safetyFrameworkRef, DebrisParent debrisParent, int debrisLayer)
    {
        _safetyFrameworkRef = safetyFrameworkRef;
        _debrisParent = debrisParent;
        _debrisLayer = debrisLayer;
    }

    public void TryThrow()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            _safetyFrameworkRef.Support1.gameObject.AddComponent<Rigidbody2D>();
            _safetyFrameworkRef.Support2.gameObject.AddComponent<Rigidbody2D>();
            _debrisParent.AddToDebris(_safetyFrameworkRef.transform);
            _safetyFrameworkRef.gameObject.layer = _debrisLayer;
        }
    }
    public void ChangeScale()
    {
        if (_isBroken == false)
        {
            _safetyFrameworkRef.Support1.localScale = _scaleSupport1SafetyFrameworkAfterHit;
            _safetyFrameworkRef.Support2.localScale = _scaleSupport2SafetyFrameworkAfterHit;
        }
    }
    
}