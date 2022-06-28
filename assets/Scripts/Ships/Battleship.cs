using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleship : BaseShip
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        MaxHealth = 120;
        Health = MaxHealth;
        Armor = 10;
        Speed = 4;

        SubAttackCountBase = 5;
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

        BaseProjectile newProjectile = ShipPrefabManager.InstantiateCannonRound(BaseDamage * DamageMultiplier + DamageModifier, ProjectileSpeed, ProjectileTracking, intercept, target, transform.position);
    }
}
