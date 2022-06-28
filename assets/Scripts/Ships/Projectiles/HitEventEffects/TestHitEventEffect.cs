using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHitEventEffect : HitEventEffect
{
    public override void Execute(BaseProjectile projectile, Collision collision)
    {
        Debug.Log("on bounce effect");
    }
}
