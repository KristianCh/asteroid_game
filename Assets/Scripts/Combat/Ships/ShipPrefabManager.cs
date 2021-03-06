using Combat.Asteroid;
using Combat.Ships.Aoes;
using Combat.Ships.Projectiles;
using UnityEngine;

namespace Combat.Ships
{
    public class ShipPrefabManager : MonoBehaviour
    {
        public BaseProjectile CannonRoundPrefab;
        public BaseProjectile MissilePrefab;

        public Explosion ExplosionPrefab;

        public static ShipPrefabManager Instance;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        }

        public static BaseProjectile InstantiateProjectileByType(ProjectileType type, float damage, float speed, float tracking, Vector3 heading, BaseAsteroid targetAsteroid, Vector3 position)
        {
            BaseProjectile newProjectile = null;
            switch (type)
            {
                case ProjectileType.CannonRound:
                    newProjectile = InstantiateProjectile(damage, speed, tracking, heading, targetAsteroid, position, Instance.CannonRoundPrefab);
                    break;
                case ProjectileType.Missile:
                    newProjectile = InstantiateProjectile(damage, speed, tracking, heading, targetAsteroid, position, Instance.MissilePrefab);
                    break;
                default:
                    newProjectile = InstantiateProjectile(damage, speed, tracking, heading, targetAsteroid, position, Instance.CannonRoundPrefab);
                    break;
            }
            return newProjectile;
        }

        public static BaseProjectile InstantiateProjectile(float damage, float speed, float tracking, Vector3 heading, BaseAsteroid targetAsteroid, Vector3 position, BaseProjectile prefab)
        {
            var NewProjectile = Instantiate(prefab, position, Quaternion.identity);
            NewProjectile.Heading = heading;
            NewProjectile.TargetAsteroid = targetAsteroid;
            NewProjectile.Damage = damage;
            NewProjectile.Speed = speed;
            NewProjectile.Tracking = tracking;

            return NewProjectile;
        }

        public static Explosion InstantiateExplosion(float scale, float lifetime, float damage, float force, Vector3 position)
        {
            var NewExplosion = Instantiate(Instance.ExplosionPrefab, position, Quaternion.identity);
        
            NewExplosion.Scale = scale;
            NewExplosion.Force = force;
            NewExplosion.Lifetime = lifetime;
            NewExplosion.Damage = damage;

            NewExplosion.Shape = AoeShape.Circle;
            NewExplosion.Type = AoeType.Explosion;

            return NewExplosion;
        }
    }
}
