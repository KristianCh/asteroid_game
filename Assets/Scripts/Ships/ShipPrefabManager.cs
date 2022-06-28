using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        BaseProjectile NewProjectile = null;
        switch (type)
        {
            case ProjectileType.CannonRound:
                NewProjectile = InstantiateCannonRound(damage, speed, tracking, heading, targetAsteroid, position);
                break;
            case ProjectileType.Missile:
                NewProjectile = InstantiateMissile(damage, speed, tracking, heading, targetAsteroid, position);
                break;
            default:
                NewProjectile = InstantiateCannonRound(damage, speed, tracking, heading, targetAsteroid, position);
                break;
        }
        return NewProjectile;
    }

    public static BaseProjectile InstantiateCannonRound(float damage, float speed, float tracking, Vector3 heading, BaseAsteroid targetAsteroid, Vector3 position)
    {
        BaseProjectile NewProjectile = Instantiate(Instance.CannonRoundPrefab, position, Quaternion.identity);
        NewProjectile.Heading = heading;
        NewProjectile.TargetAsteroid = targetAsteroid;
        NewProjectile.Damage = damage;
        NewProjectile.Speed = speed;
        NewProjectile.Tracking = tracking;

        return NewProjectile;
    }

    public static BaseProjectile InstantiateMissile(float damage, float speed, float tracking, Vector3 heading, BaseAsteroid targetAsteroid, Vector3 position)
    {
        BaseProjectile NewProjectile = Instantiate(Instance.MissilePrefab, position, Quaternion.identity);
        NewProjectile.Heading = heading;
        NewProjectile.TargetAsteroid = targetAsteroid;
        NewProjectile.Damage = damage;
        NewProjectile.Speed = speed;
        NewProjectile.Tracking = tracking;

        return NewProjectile;
    }

    public static Explosion InstantiateExplosion(float scale, float lifetime, float damage, float force, Vector3 position)
    {
        Explosion NewExplosion = Instantiate(Instance.ExplosionPrefab, position, Quaternion.identity);
        NewExplosion.SetScale(scale);

        NewExplosion.Force = force;
        NewExplosion.Lifetime = lifetime;
        NewExplosion.Damage = damage;

        NewExplosion.Shape = AoeShape.Circle;
        NewExplosion.Type = AoeType.Explosion;

        return NewExplosion;
    }
}
