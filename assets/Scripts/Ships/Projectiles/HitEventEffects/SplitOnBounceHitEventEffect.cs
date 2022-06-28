using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitOnBounceHitEventEffect : HitEventEffect
{
    public override void Execute(BaseProjectile projectile, Collision collision)
    {
        Quaternion left = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1));
        Quaternion right = Quaternion.AngleAxis(30, new Vector3(0, 0, 1));

        BaseProjectile leftP = ShipPrefabManager.InstantiateProjectileByType(
            projectile.Type,
            projectile.Damage / 2,
            projectile.Speed / 2,
            projectile.Tracking / 2,
            left * projectile.Heading,
            projectile.TargetAsteroid,
            projectile.transform.position + left * projectile.Heading * 0.1f
        );

        BaseProjectile rightP = ShipPrefabManager.InstantiateProjectileByType(
            projectile.Type,
            projectile.Damage / 2,
            projectile.Speed / 2,
            projectile.Tracking / 2,
            right * projectile.Heading,
            projectile.TargetAsteroid,
            projectile.transform.position + right * projectile.Heading * 0.1f
        );

        leftP.transform.localScale *= 0.5f;
        rightP.transform.localScale *= 0.5f;

        leftP.BouncesLeft = 0;
        rightP.BouncesLeft = 0;
    }
}
