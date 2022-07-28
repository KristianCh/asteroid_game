using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseShip : MonoBehaviour
{
    public ShipDataScriptableObject ShipData;
    public Image HealthBar;
    public Image CooldownBar;
    private float HealthBarFillAmount = 1;
    private float CooldownBarFillAmount = 1;

    private bool IsFlagship = false;

    protected float Health = 100;
    protected int Level = 1;
    protected float AttackCooldownTimer = 5;
    protected float SubAttackCooldownTimer = 0.2f;

    protected bool MainCooldownTriggered = false;
    protected int SubAttackCount = 1;
    protected int LayerMask = 1 << 8;

    protected Vector3 Heading = new Vector3(0, 1, 0);

    protected List<StatusEffect> AppliedStatusEffects = new List<StatusEffect>();

    // Start is called before the first frame update
    public virtual void Start()
    {
        Fleet.Instance.AddToFleet(this);
        Health = ShipData.MaxHealth[Level];
        AttackCooldownTimer = ShipData.AttackCooldown[Level];
        SubAttackCount = ShipData.SubAttackCount[Level];
    }

    // Update is called once per frame
    public virtual void Update()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, HealthBarFillAmount, Time.deltaTime * 10);
        CooldownBar.fillAmount = Mathf.Lerp(CooldownBar.fillAmount, CooldownBarFillAmount, Time.deltaTime * 10);

        CalculateMovement();
        ProcessAttack();
    }

    protected virtual void CalculateMovement()
    {
        // Save old heading, slerp to target
        Vector3 oldHeading = Heading;
        Heading = Vector3.Slerp(Heading, (Fleet.Instance.GetTargetPosition() - transform.position).normalized, ShipData.TurnSpeed * Time.deltaTime).normalized;

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
            if (Physics.Raycast(transform.position, raycastDirection, out hit, ShipData.VisibilityRange, LayerMask))
            {
                // Weight of distance on steering
                float distFactor = (ShipData.VisibilityRange - hit.distance) / ShipData.VisibilityRange;
                // Weight of angle on steering
                float angleFactor = 1.0f - Mathf.Floor(Mathf.Abs(iVal)) * 0.25f;
                // Steering away from hit weighted by distance and angle 
                Heading = Vector3.Slerp(Heading, (transform.position - hit.point).normalized, ShipData.TurnSpeed * Time.deltaTime * distFactor * angleFactor).normalized;
            }
        }

        // Calculate new position
        Vector3 NewPos = transform.position + Heading * ShipData.Speed * Time.deltaTime;
        NewPos.z = 0;
        transform.position = NewPos;
        
        // Calculate tilt and heading rotations
        float Angle = Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg - 90;
        float ChangeInAngle = Vector3.SignedAngle(oldHeading, Heading, new Vector3(0, 0, 1)) * 10;
        transform.rotation = Quaternion.AngleAxis(Angle, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(ChangeInAngle, Vector3.up);
    }

    protected virtual void OnCooldown()
    {
        AttackCooldownTimer = ShipData.AttackCooldown[Level];
        MainCooldownTriggered = true;
    }

    protected virtual void OnSubCooldown()
    {
        SubAttackCooldownTimer = ShipData.SubAttackCooldown;
        if (SubAttackCount > 0)
        {
            SubAttackCount--;
        }
        if (SubAttackCount == 0)
        {
            SubAttackCount = ShipData.SubAttackCount[Level];
            MainCooldownTriggered = false;
        }
    }

    protected virtual void ProcessAttack()
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
            CooldownBarFillAmount = 1 - AttackCooldownTimer / ShipData.AttackCooldown[Level];
            if (AttackCooldownTimer <= 0)
            {
                OnCooldown();
            }
        }
    }

    public virtual void ProcessDamage(float damage)
    {
        damage = Mathf.Max(damage - ShipData.Armor[Level]);
        Health -= Mathf.Max(0, damage);
        HealthBarFillAmount = Health / ShipData.MaxHealth[Level];

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public static BaseAsteroid GetClosestAsteroid(Vector3 position)
    {
        BaseAsteroid closest = null;
        foreach (BaseAsteroid asteroid in GameManager.Instance.ActiveAsteroids)
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

    protected static Vector3 GetAsteroidInterceptVector(Vector3 position, BaseAsteroid asteroid, float projectileSpeed)
    {
        Vector3 asteroidPos = asteroid.transform.position;
        Vector3 asteroidVel = asteroid.asteroidRigidbody.velocity;

        Vector3 InterceptVector = ( 
                asteroidPos - position + 
                asteroidVel * ( (asteroidPos - position).magnitude / projectileSpeed )
            ).normalized;

        return InterceptVector;
    }

    public virtual string GetStoreTitle()
    {
        return "";
    }

    public virtual string GetStoreDescription()
    {
        return "";
    }
}
