using System.Collections.Generic;
using Combat.Asteroid;
using Combat.Ships.Projectiles.HitEventEffects;
using UnityEngine;

namespace Combat.Ships.Projectiles
{
    public enum ProjectileType
    {
        CannonRound,
        Missile
    }

    public class BaseProjectile : ShipSpawnableObject
    {
        // TODO: Use scriptable object for this
        public ProjectileType Type;

        public float Speed = 10;
        public float Tracking = 0;
        public float Damage = 10;
        public float Lifetime = -1;

        public int BouncesLeft = 0;
        public int PiercesLeft = 0;

        public List<AHitEventEffect> BounceEffects = new List<AHitEventEffect>();
        public List<AHitEventEffect> PierceEffects = new List<AHitEventEffect>();
        public List<AHitEventEffect> HitEffects = new List<AHitEventEffect>();
        public List<AHitEventEffect> DeathEffects = new List<AHitEventEffect>();

        public Vector3 Heading = new Vector3(0, 1, 0);
        public BaseAsteroid TargetAsteroid = null;

        public bool Retarget = false;

        // Start is called before the first frame update
        public virtual void Start()
        {
            BounceEffects.Add(new SplitOnBounceAHitEventEffect());
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (Lifetime > -1)
            {
                Lifetime -= Time.deltaTime;
                if (Lifetime < 0)
                {
                    Death();
                }
            }

            if (transform.position.magnitude > Camera.main.orthographicSize * Camera.main.aspect * 2)
            {
                Destroy(gameObject);
            }

            var oldHeading = Heading;
            if ((TargetAsteroid == null || TargetAsteroid.IsDead) && Retarget)
            {
                TargetAsteroid = GameManager.GetClosestAsteroid(transform.position);
            }

            if (TargetAsteroid != null && Tracking > 0)
            {
                Heading = Vector3.Slerp(Heading, (TargetAsteroid.transform.position - transform.position).normalized, Tracking * Time.deltaTime);
            }
            // Calculate new position
            var newPos = transform.position + Heading * (Speed * Time.deltaTime);
            newPos.z = 0;
            transform.position = newPos;
            // Calculate new rotation
            var angle = Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg - 90;
            var changeInAngle = Vector3.SignedAngle(oldHeading, Heading, new Vector3(0, 0, 1)) * 10 + (10 * Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(changeInAngle, Vector3.up);
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            var asteroid = collision.gameObject.GetComponent(typeof(BaseAsteroid)) as BaseAsteroid;

            if (asteroid != null)
            {
                asteroid.Damage(
                    new DamageInfo(Damage, DamageType.Kinetic, collision.contacts[0].point, true)
                );
            }

            ExecuteHitEventEffects(HitEffects, collision);
            if (BouncesLeft > 0)
            {
                Bounce(collision);
            }
            else if (PiercesLeft > 0)
            {
                Pierce(collision);
            }
            else
            {
                Death();
            }
        }

        protected virtual void Bounce(Collision collision)
        {
            foreach (var contact in collision.contacts)
            {
                var normal = contact.normal;
                normal.z = 0;
                normal = normal.normalized;

                var reflect = 2 * Vector3.Dot(Heading, -normal) * normal - Heading;
                Heading = reflect.normalized;

                break;
            }
            ExecuteHitEventEffects(BounceEffects, collision);
            BouncesLeft--;
        }

        protected virtual void Pierce(Collision collision)
        {
            ExecuteHitEventEffects(PierceEffects, collision);
            PiercesLeft--;
        }

        protected virtual void Death()
        {
            ExecuteHitEventEffects(DeathEffects, null);
            Destroy(gameObject);
        }

        protected void ExecuteHitEventEffects(List<AHitEventEffect> hitEventEffects, Collision collision)
        {
            foreach (AHitEventEffect effect in hitEventEffects)
            {
                effect.Execute(this, collision);
            }
        }
    }
}