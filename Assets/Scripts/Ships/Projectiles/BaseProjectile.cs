using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    CannonRound,
    Missile
}

public class BaseProjectile : ShipSpawnableObject
{
    public ProjectileType Type;

    public float Speed = 10;
    public float Tracking = 0;
    public float Damage = 10;
    public float Lifetime = -1;

    public int BouncesLeft = 0;
    public int PiercesLeft = 0;

    public List<HitEventEffect> BounceEffects = new List<HitEventEffect>();
    public List<HitEventEffect> PierceEffects = new List<HitEventEffect>();
    public List<HitEventEffect> HitEffects = new List<HitEventEffect>();
    public List<HitEventEffect> DeathEffects = new List<HitEventEffect>();

    public Vector3 Heading = new Vector3(0, 1, 0);
    public BaseAsteroid TargetAsteroid = null;

    public bool Retarget = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        BounceEffects.Add(new SplitOnBounceHitEventEffect());
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

        Vector3 oldHeading = Heading;
        if ((TargetAsteroid == null || TargetAsteroid.IsDead) && Retarget)
        {
            TargetAsteroid = BaseShip.GetClosestAsteroid(transform.position);
        }

        if (TargetAsteroid != null && Tracking > 0)
        {
            Heading = Vector3.Slerp(Heading, (TargetAsteroid.transform.position - transform.position).normalized, Tracking * Time.deltaTime);
        }
        // Calculate new position
        Vector3 NewPos = transform.position + Heading * Speed * Time.deltaTime;
        NewPos.z = 0;
        transform.position = NewPos;
        // Calculate new rotation
        float Angle = Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg - 90;
        float ChangeInAngle = Vector3.SignedAngle(oldHeading, Heading, new Vector3(0, 0, 1)) * 10 + (10 * Time.deltaTime);
        transform.rotation = Quaternion.AngleAxis(Angle, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(ChangeInAngle, Vector3.up);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        BaseAsteroid asteroid = collision.gameObject.GetComponent(typeof(BaseAsteroid)) as BaseAsteroid;

        if (asteroid != null)
        {
            asteroid.Damage(Damage, DamageType.Kinetic, collision.contacts[0].point);
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

    public virtual void Bounce(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 normal = contact.normal;
            normal.z = 0;
            normal = normal.normalized;

            Vector3 reflect = 2 * Vector3.Dot(Heading, -normal) * normal - Heading;
            Heading = reflect.normalized;

            break;
        }
        ExecuteHitEventEffects(BounceEffects, collision);
        BouncesLeft--;
    }

    public virtual void Pierce(Collision collision)
    {
        ExecuteHitEventEffects(PierceEffects, collision);
        PiercesLeft--;
    }

    public virtual void Death()
    {
        ExecuteHitEventEffects(DeathEffects, null);
        Destroy(gameObject);
    }

    protected void ExecuteHitEventEffects(List<HitEventEffect> HitEventEffects, Collision collision)
    {
        foreach (HitEventEffect effect in HitEventEffects)
        {
            effect.Execute(this, collision);
        }
    }
}
