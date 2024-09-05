
using UnityEngine;

public class StandartGlassDestructionHandler : GlassDestructionHandler
{
    public StandartGlassDestructionHandler(GlassRef glassRef,
        DestructionHandlerContent destructionHandlerContent, DestructionEffectsHandler destructionEffectsHandler,
        int layerLevelContent, int layerNotDestructibleLevelContent) 
        : base(glassRef, destructionHandlerContent ,destructionEffectsHandler, layerLevelContent, layerNotDestructibleLevelContent)
    {
        SubscribeCollider(GlassNormal.GetComponent<Collider2D>(), CheckCollision, TryBreakGlassFromHit);
    }
    
    public override void TryBreakGlassFromWings()
    {
        if (IsBreaked == false)
        {
            TryBreakGlass(CurrentGlass.transform.position);
        }
    }
    public override void TryThrowGlass()
    {
        if (IsBroken == false)
        {
            IsBroken = true;
            TryBreakGlassFromHit();
            CompositeDisposable.Clear();
            RigidBodySetSimulated(Rigidbody2DCurrentGlass);
            SetParentDebris();
            SetCarDebrisLayerInteractableWithCar();
            SetIgnoreLayerLevelContent();
        }
    }
    protected override void TryBreakGlassFromHit()
    {
        if (IsBreaked == false)
        {
            TryBreakGlass(HitPosition);
        }
    }
    protected override void TryBreakGlass(Vector2 position)
    {
        if (IsBreaked == false)
        {
            IsBreaked = true;
            DestructionEffectsHandler.GlassBrokenEffect(position);
            CompositeDisposable.Clear();
            TrySwitchSprites();
            SetIgnoreLayerLevelContent();
        }
    }
    private void TrySwitchSprites()
    {
        if (GlassDamaged != null)
        {
            GlassNormal.gameObject.SetActive(false);
            GlassDamaged.gameObject.SetActive(true);
            CurrentGlass = GlassDamaged;
            Rigidbody2DCurrentGlass = Rigidbody2DGlassDamaged;
        }
    }
    private void SetIgnoreLayerLevelContent()
    {
        Physics2D.IgnoreLayerCollision(GlassRef.gameObject.layer,LayerLevelContent,true);
        Physics2D.IgnoreLayerCollision(GlassRef.gameObject.layer,LayerNotDestructibleLevelContent, true);
    }
}