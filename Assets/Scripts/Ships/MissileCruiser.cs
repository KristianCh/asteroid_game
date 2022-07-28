using Ships.Projectiles;

namespace Ships
{
    public class MissileCruiser : BaseShip
    {
        protected override void OnSubCooldown()
        {
            base.OnSubCooldown();

            var target = GetClosestAsteroid(transform.position);
            if (target == null) return;

            var intercept = GetAsteroidInterceptVector(transform.position, target, ShipData.ProjectileSpeed);

            var newProjectile = ShipPrefabManager.InstantiateProjectileByType(
                ProjectileType.Missile,
                ShipData.Damage[Level],
                ShipData.ProjectileSpeed,
                ShipData.ProjectileTracking,
                intercept,
                target,
                transform.position
            );
        }
    }
}
