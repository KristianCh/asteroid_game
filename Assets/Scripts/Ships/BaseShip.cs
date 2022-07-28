using System.Collections;
using System.Collections.Generic;
using Asteroid;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
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
            var oldHeading = Heading;
            Heading = Vector3.Slerp(Heading, (Fleet.Instance.GetTargetPosition() - transform.position).normalized, ShipData.TurnSpeed * Time.deltaTime).normalized;

            // Perform collision avoidance raycasts
            for (var i = -2; i <= 1; i++)
            {
                // Offset of raycast from center
                var iVal = (float)i + 0.5f;
                // Offset angle for raycast
                var castAngle = iVal * 45;

                // Rotate look direction vector by angle around Y axis
                var raycastDirection = Quaternion.AngleAxis(castAngle, new Vector3(0, 0, 1)) * Heading;
                // Resolve raycast hit
                if (!Physics.Raycast(transform.position, raycastDirection, out var hit, ShipData.VisibilityRange,
                        LayerMask)) continue;
                // Weight of distance on steering
                var distFactor = (ShipData.VisibilityRange - hit.distance) / ShipData.VisibilityRange;
                // Weight of angle on steering
                var angleFactor = 1.0f - Mathf.Floor(Mathf.Abs(iVal)) * 0.25f;
                // Steering away from hit weighted by distance and angle 
                Heading = Vector3.Slerp(Heading, (transform.position - hit.point).normalized, ShipData.TurnSpeed * Time.deltaTime * distFactor * angleFactor).normalized;
            }

            // Calculate new position
            var newPos = transform.position + Heading * (ShipData.Speed * Time.deltaTime);
            newPos.z = 0;
            transform.position = newPos;
        
            // Calculate tilt and heading rotations
            var angle = Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg - 90;
            var changeInAngle = Vector3.SignedAngle(oldHeading, Heading, new Vector3(0, 0, 1)) * 10;
            transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(changeInAngle, Vector3.up);
        }

        protected virtual void OnCooldown()
        {
            AttackCooldownTimer = ShipData.AttackCooldown[Level];
            MainCooldownTriggered = true;
        
            StartCoroutine(SubCooldownRoutine());
        }

        protected virtual void OnSubCooldown()
        {
        
        }

        private IEnumerator SubCooldownRoutine()
        {
            for (var i = 0; i < ShipData.SubAttackCount[Level]; i++)
            {
                OnSubCooldown();
                yield return new WaitForSeconds(ShipData.SubAttackCooldown);
            }
            MainCooldownTriggered = false;
        }

        protected virtual void ProcessAttack()
        {
            if (MainCooldownTriggered) return;
        
            AttackCooldownTimer -= Mathf.Max(0, Time.deltaTime);
            CooldownBarFillAmount = 1 - AttackCooldownTimer / ShipData.AttackCooldown[Level];
            if (AttackCooldownTimer <= 0)
            {
                OnCooldown();
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
            var asteroidPos = asteroid.transform.position;
            var asteroidVel = asteroid.asteroidRigidbody.velocity;

            var InterceptVector = ( 
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
}
