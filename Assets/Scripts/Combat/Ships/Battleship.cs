using Combat.Ships.Projectiles;
using Data;

namespace Combat.Ships
{
    public abstract class Battleship<TData> : BaseShip<TData> where TData : BaseShipData
    {
        protected override void OnSubCooldown()
        {
            base.OnSubCooldown();

            var target = GameManager.GetClosestAsteroid(transform.position);
            if (target == null) return;

            var intercept = GetAsteroidInterceptVector(transform.position, target, ShipData.ProjectileSpeed);

            var newProjectile = ShipPrefabManager.InstantiateProjectileByType(
                ProjectileType.CannonRound, 
                ShipData.Damage[Level],
                ShipData.ProjectileSpeed,
                ShipData.ProjectileTracking, 
                intercept, 
                target, 
                transform.position
            );
        }
    }
    
    public class Battleship : Battleship<BaseShipData> { }
}
