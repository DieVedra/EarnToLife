using System;
using UnityEngine;

public abstract class GlassDestructionHandler : DestructionHandler, IDispose
{
    protected readonly GlassRef GlassRef;
    protected readonly DestructionEffectsHandler DestructionEffectsHandler;
    protected readonly int LayerLevelContent;
    protected readonly int LayerNotDestructibleLevelContent;
    protected Transform GlassNormal;
    protected Transform GlassDamaged;
    protected Transform CurrentGlass;
    protected Rigidbody2D Rigidbody2DGlassNormal;
    protected Rigidbody2D Rigidbody2DGlassDamaged;
    protected Rigidbody2D Rigidbody2DCurrentGlass;
    protected bool IsBreaked = false;
    protected bool IsBroken = false;
    protected GlassDestructionHandler(GlassRef glassRef, DestructionHandlerContent destructionHandlerContent, DestructionEffectsHandler destructionEffectsHandler,
        int layerLevelContent, int layerNotDestructibleLevelContent)
    :base(glassRef, destructionHandlerContent, " GlassDestruction ", maxStrength: glassRef.StrengthGlass)
    {
        TryInitGlasses(glassRef);
        GlassRef = glassRef;
        DestructionEffectsHandler = destructionEffectsHandler;
        LayerLevelContent = layerLevelContent;
        LayerNotDestructibleLevelContent = layerNotDestructibleLevelContent;
    }
    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
    public abstract void TryThrowGlass();
    public abstract void TryBreakGlassFromWings();
    protected abstract void TryBreakGlass(Vector2 position);
    protected abstract void TryBreakGlassFromHit();

    // protected void TrySwitchSprites()
    // {
    //     if (GlassDamaged != null)
    //     {
    //         GlassNormal.gameObject.SetActive(false);
    //         GlassDamaged.gameObject.SetActive(true);
    //         CurrentGlass = GlassDamaged;
    //     }
    // }
    private void TryInitGlasses(GlassRef glassRef)
    {
        GlassNormal = glassRef.Glasses[0];
        Rigidbody2DGlassNormal = GetNotSimulatedRigidBodyOrTryAdd(glassRef.Glasses[0].gameObject);
        CurrentGlass = GlassNormal;
        Rigidbody2DCurrentGlass = Rigidbody2DGlassNormal;
        if (glassRef.Glasses.Length > 1)
        {
            GlassDamaged = glassRef.Glasses[1];
            Rigidbody2DGlassDamaged = GetNotSimulatedRigidBodyOrTryAdd(glassRef.Glasses[1].gameObject);
        }
    }
}