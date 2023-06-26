using UnityEngine;

namespace Combat.Ships.Projectiles.HitEventEffects
{
    public abstract class AHitEventEffect
    {
        public virtual void Execute(BaseProjectile projectile, Collision collision)
        {

        }
    }
}
