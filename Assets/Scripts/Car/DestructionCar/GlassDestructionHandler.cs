using UnityEngine;

public class GlassDestructionHandler : DestructionHandler
{
    private Transform _glassNormal;
    private Transform _glassDamaged;
    private bool _isBreaked = false;
    private bool _isBroken = false;
    public GlassDestructionHandler(GlassRef glassRef, DestructionHandlerContent destructionHandlerContent)
    :base(glassRef, destructionHandlerContent, glassRef.StrengthGlass)
    {
        TryInitGlasses(glassRef);
        SubscribeCollider(_glassNormal.GetComponent<Collider2D>(), CheckCollision, TryBreakGlass);
    }
    public void TryThrowGlass()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            TryBreakGlass();
            CompositeDisposable.Clear();
            _glassDamaged.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris();
            SetCarDebrisLayer();
        }
    }

    public void TryBreakGlass()
    {
        if (_isBreaked == false)
        {
            _isBreaked = true;
            CompositeDisposable.Clear();
            SwitchSprites();
        }
    }

    private void SwitchSprites()
    {
        if (_glassDamaged != null)
        {
            _glassNormal.gameObject.SetActive(false);
            _glassDamaged.gameObject.SetActive(true);
        }
    }

    private void TryInitGlasses(GlassRef glassRef)
    {
        _glassNormal = glassRef.Glasses[0];
        if (glassRef.Glasses.Length > 1)
        {
            _glassDamaged = glassRef.Glasses[1];
        }
    }
}