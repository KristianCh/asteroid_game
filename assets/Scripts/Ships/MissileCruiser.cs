using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCruiser : BaseShip
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        MaxHealth = 100;
        Health = MaxHealth;
        Armor = 5;
        Speed = 5;
        ProjectileSpeed = 5;
        ProjectileTracking = 3;

        SubAttackCountBase = 3;
        SubAttackCount = SubAttackCountBase;
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
