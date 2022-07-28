using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCruiser : BaseShip
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    protected override void OnSubCooldown()
    {
        base.OnSubCooldown();

        BaseAsteroid target = GetClosestAsteroid(transform.position);
        if (target == null) return;

        Vector3 intercept = GetAsteroidInterceptVector(transform.position, target, ShipData.ProjectileSpeed);

        BaseProjectile newProjectile = ShipPrefabManager.InstantiateProjectileByType(
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
