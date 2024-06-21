
using UnityEngine;

public class ArmoredGlassDestructionHandler : GlassDestructionHandler
{
    public ArmoredGlassDestructionHandler(GlassRef glassRef,
        DestructionHandlerContent destructionHandlerContent, DestructionEffectsHandler destructionEffectsHandler,
        int layerLevelContent, int layerNotDestructibleLevelContent) 
        : base(glassRef, destructionHandlerContent ,destructionEffectsHandler, layerLevelContent, layerNotDestructibleLevelContent)
    {
        SubscribeCollider(GlassNormal.GetComponent<Collider2D>(), CheckCollision, TryBreakGlassFromHit);
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
        }
    }

    public override void TryBreakGlassFromWings()
    {
        if (IsBreaked == false)
        {
            TryBreakGlass(CurrentGlass.transform.position);
        }
    }

    protected override void TryBreakGlass(Vector2 position)
    {
        if (IsBreaked == false)
        {
            IsBreaked = true;
            DestructionEffectsHandler.HitBrokenEffect(position, ImpulseNormalValue);
            CompositeDisposable.Clear();
        }
    }

    protected override void TryBreakGlassFromHit()
    {
        if (IsBreaked == false)
        {
            TryBreakGlass(HitPosition);
        }
    }
}