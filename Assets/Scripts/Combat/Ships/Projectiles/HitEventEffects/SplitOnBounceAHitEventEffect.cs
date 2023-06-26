using System.Numerics;
using UnityEngine;

namespace Combat.Ships.Projectiles.HitEventEffects
{
    public class SplitOnBounceAHitEventEffect : AHitEventEffect
    {
        public override void Execute(BaseProjectile projectile, Collision collision)
        {
            var left = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1));
            var right = Quaternion.AngleAxis(30, new Vector3(0, 0, 1));

            var leftP = ShipPrefabManager.InstantiateProjectileByType(
                projectile.Type,
                projectile.Damage / 2,
                projectile.Speed / 2,
                projectile.Tracking / 2,
                left * projectile.Heading,
                projectile.TargetAsteroid,
                projectile.transform.position + left * projectile.Heading * 0.1f
            );

            var rightP = ShipPrefabManager.InstantiateProjectileByType(
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
}
