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

    public override void OnSubCooldown()
    {
        base.OnSubCooldown();

        BaseAsteroid target = GetClosestAsteroid(transform.position);
        if (target == null) return;

        Vector3 intercept = GetAsteroidInterceptVector(transform.position, target, ProjectileSpeed);

        ShipPrefabManager.InstantiateMissile(BaseDamage * DamageMultiplier + DamageModifier, ProjectileSpeed, ProjectileTracking, intercept, target, transform.position);
    }
}
