using UnityEngine;

public class SafetyFrameworkDestructionHandler
{
    private readonly Vector3 _scaleSupport1SafetyFrameworkAfterHit = new Vector3(1f, 0.7f,1f);
    private readonly Vector3 _scaleSupport2SafetyFrameworkAfterHit = new Vector3(1f, 0.62f,1f);
    private readonly Transform _debrisParent;
    public readonly SafetyFrameworkRef SafetyFrameworkRef;
    private bool _isBroken;
    
    public SafetyFrameworkDestructionHandler(SafetyFrameworkRef safetyFrameworkRef, Transform debrisParent)
    {
        SafetyFrameworkRef = safetyFrameworkRef;
        _debrisParent = debrisParent;
    }

    public void TryThrow()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            SafetyFrameworkRef.Support1.gameObject.AddComponent<Rigidbody2D>();
            SafetyFrameworkRef.Support2.gameObject.AddComponent<Rigidbody2D>();
            SafetyFrameworkRef.transform.SetParent(_debrisParent);
        }
    }
    public void ChangeScale()
    {
        if (_isBroken == false)
        {
            SafetyFrameworkRef.Support1.localScale = _scaleSupport1SafetyFrameworkAfterHit;
            SafetyFrameworkRef.Support2.localScale = _scaleSupport2SafetyFrameworkAfterHit;
        }
    }
    
}