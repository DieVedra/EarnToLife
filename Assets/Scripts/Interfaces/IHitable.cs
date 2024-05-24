using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public Vector2 Position { get; }
    public bool IsBroken { get; }
    public bool TryBreakOnImpact(float forceHit);
}