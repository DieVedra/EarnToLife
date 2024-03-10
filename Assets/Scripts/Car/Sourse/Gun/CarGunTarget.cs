using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGunTarget
{
    public Collider2D TargetCollider { get; private set; }
    public IShotable Target { get; private set; }
    public CarGunTarget(IShotable currentTarget, Collider2D currentTargetCollider)
    {
        TargetCollider = currentTargetCollider;
        Target = currentTarget;
    }
}
