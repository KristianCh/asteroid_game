using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseShip : MonoBehaviour
{
    public Image HealthBar;
    public Image CooldownBar;
    private float HealthBarFillAmount = 1;
    private float CooldownBarFillAmount = 1;

    public bool IsFlagship = false;

    public float Health = 100;
    public float MaxHealth = 100;
    public float Armor = 0;
    public float Speed = 5;
    public float TurnSpeed = 3;
    public float VisibilityRange = 1;
    public float TargetingRange = 100;
    public float Level = 1;
    public float SubLevel = 1;
    public float AttackCooldown = 5;
    public float SubAttackCooldown = 0.2f;
    public float AttackCooldownTimer = 5;
    public float SubAttackCooldownTimer = 0.2f;
    public float BaseDamage = 10;
    public float DamageMultiplier = 1;
    public float DamageModifier = 0;
    public float ProjectileSpeed = 10;
    public float ProjectileTracking = 0;

    public bool MainCooldownTriggered = false;
    public int SubAttackCountBase = 1;
    public int SubAttackCount = 1;
    public int LayerMask = 1 << 8;

    public Vector3 Heading = new Vector3(0, 1, 0);

    public List<string> Classes = new List<string>();
    public List<StatusEffect> AppliedStatusEffects = new List<StatusEffect>();

    private bool SetFleet = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        AttackCooldownTimer = AttackCooldown;
        SubAttackCooldownTimer = SubAttackCooldown;

        SubAttackCount = SubAttackCountBase;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!SetFleet && Fleet.Instance != null)
        {
            SetFleet = true;
            Fleet.Instance.ShipList.Add(this);
        }

        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, HealthBarFillAmount, Time.deltaTime * 10);
        CooldownBar.fillAmount = Mathf.Lerp(CooldownBar.fillAmount, CooldownBarFillAmount, Time.deltaTime * 10);

        CalculateMovement();
        ProcessAttack();
    }

    public virtual void CalculateMovement()
    {
        // Save old heading, slerp to target
        Vector3 oldHeading = Heading;
        Heading = Vector3.Slerp(Heading, (Fleet.Instance.TargetPosition - transform.position).normalized, TurnSpeed * Time.deltaTime).normalized;

        // Perform collision avoidance raycasts
        for (int i = -2; i <= 1; i++)
        {
            // Offset of raycast from center
            float iVal = (float)i + 0.5f;
            // Offset angle for raycast
            float angle = iVal * 45;

            // Rotate look direction vector by angle around Y axis
            Vector3 raycastDirection = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)) * Heading;
            RaycastHit hit;
            // Resolve raycast hit
            if (Physics.Raycast(transform.position, raycastDirection, out hit, VisibilityRange, LayerMask))
            {
                // Weight of distance on steering
                float distFactor = (VisibilityRange - hit.distance) / VisibilityRange;
                // Weight of angle on steering
                float angleFactor = 1.0f - Mathf.Floor(Mathf.Abs(iVal)) * 0.25f;
                // Steering away from hit weighted by distance and angle 
                Heading = Vector3.Slerp(Heading, (transform.position - hit.point).normalized, TurnSpeed * Time.deltaTime * distFactor * angleFactor).normalized;

                Debug.DrawRay(transform.position, raycastDirection * hit.distance, new Color(1, 1 - distFactor * angleFactor, 1 - distFactor * angleFactor, 1));
            }
            else
            {
                Debug.DrawRay(transform.position, raycastDirection * VisibilityRange, Color.white);
            }
        }

        // Calculate new position
        Vector3 NewPos = transform.position + Heading * Speed * Time.deltaTime;
        NewPos.z = 0;
        transform.position = NewPos;
        
        // Calculate tilt and heading rotations
        float Angle = Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg - 90;
        float ChangeInAngle = Vector3.SignedAngle(oldHeading, Heading, new Vector3(0, 0, 1)) * 10;
        transform.rotation = Quaternion.AngleAxis(Angle, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(ChangeInAngle, Vector3.up);
    }

    public virtual void OnCooldown()
    {
        AttackCooldownTimer = AttackCooldown;
        MainCooldownTriggered = true;
    }

    public virtual void OnSubCooldown()
    {
        SubAttackCooldownTimer = SubAttackCooldown;
        if (SubAttackCount > 0)
        {
            SubAttackCount--;
        }
        if (SubAttackCount == 0)
        {
            SubAttackCount = SubAttackCountBase;
            MainCooldownTriggered = false;
        }
    }

    public virtual void ProcessAttack()
    {
        if (MainCooldownTriggered)
        {
            SubAttackCooldownTimer -= Time.deltaTime;
            if (SubAttackCooldownTimer <= 0)
            {
                OnSubCooldown();
            }
        }
        else
        {
            AttackCooldownTimer -= Mathf.Max(0, Time.deltaTime);
            CooldownBarFillAmount = 1 - AttackCooldownTimer / AttackCooldown;
            if (AttackCooldownTimer <= 0)
            {
                OnCooldown();
            }
        }
    }

    public virtual void Damage(float damage)
    {
        damage = Mathf.Max(damage - Armor);
        Health -= Mathf.Max(0, damage);
        HealthBarFillAmount = Health / MaxHealth;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public static BaseAsteroid GetClosestAsteroid(Vector3 position)
    {
        BaseAsteroid closest = null;
        foreach (BaseAsteroid asteroid in WaveManager.Instance.ActiveAsteroids)
        {
            if (closest == null)
            {
                if (!asteroid.IsDead)
                {
                    closest = asteroid;
                }
            }
            else if ((closest.transform.position - position).sqrMagnitude > (asteroid.transform.position - position).sqrMagnitude && !asteroid.IsDead)
            {
                closest = asteroid;
            }
        }
        return closest;
    }

    public static Vector3 GetAsteroidInterceptVector(Vector3 position, BaseAsteroid asteroid, float projectileSpeed)
    {
        Vector3 asteroidPos = asteroid.transform.position;
        Vector3 asteroidVel = asteroid.asteroidRigidbody.velocity;

        Vector3 InterceptVector = ( 
                asteroidPos - position + 
                asteroidVel * ( (asteroidPos - position).magnitude / projectileSpeed )
            ).normalized;

        return InterceptVector;
    }
}
