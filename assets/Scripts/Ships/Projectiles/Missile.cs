using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : BaseProjectile
{
    public override void OnCollisionEnter(Collision collision)
    {
        BaseAsteroid asteroid = collision.gameObject.GetComponent(typeof(BaseAsteroid)) as BaseAsteroid;

        if (asteroid != null)
        {
            ShipPrefabManager.InstantiateExplosion(0.5f, 1, Damage, 100, transform.position);
        }

        ExecuteHitEventEffects(HitEffects, collision);
        if (BouncesLeft > 0)
        {
            Bounce(collision);
        }
        else if (PiercesLeft > 0)
        {
            Pierce(collision);
        }
        else
        {
            Death();
        }
    }
}
