using System.Collections.Generic;
using UnityEngine;

public interface IExplosive
{
    public Vector2 Position { get; }
    public IReadOnlyList<DebrisFragment> DebrisFragments { get; }

    public bool TryBreakOnExplosion(Vector2 direction, float forceHit);
}