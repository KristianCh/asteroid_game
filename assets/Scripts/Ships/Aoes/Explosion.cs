using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : BaseAoe
{
    public float Damage = 20;
    public float Force = 50;

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

    public virtual void OnTriggerEnter(Collider collider)
    {
        BaseAsteroid asteroid = collider.gameObject.GetComponent(typeof(BaseAsteroid)) as BaseAsteroid;
        if (asteroid != null)
        {
            asteroid.Damage(Damage, DamageType.Explosive);
        }

        collider.attachedRigidbody.AddForceAtPosition((asteroid.transform.position - transform.position).normalized * Force, asteroid.transform.position);

    }
}
