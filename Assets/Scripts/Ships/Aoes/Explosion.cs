using Asteroid;
using UnityEngine;

namespace Ships.Aoes
{
    public class Explosion : BaseAoe
    {
        public float Damage = 20;
        public float Force = 50;

        public virtual void OnTriggerEnter(Collider collider)
        {
            var asteroid = collider.gameObject.GetComponent(typeof(BaseAsteroid)) as BaseAsteroid;
            if (asteroid != null)
            {
                asteroid.Damage(Damage, DamageType.Explosive, asteroid.transform.position);
            }

            collider.attachedRigidbody.AddForceAtPosition((asteroid.transform.position - transform.position).normalized * Force, asteroid.transform.position);

        }
    }
}
