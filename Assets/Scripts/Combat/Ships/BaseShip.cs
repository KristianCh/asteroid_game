using System.Collections;
using System.Collections.Generic;
using Combat.Asteroid;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.Ships
{
    public abstract class BaseShip : MonoBehaviour, IDamagable
    {
        [SerializeField]
        public Image HealthBar;
        [SerializeField]
        public Image CooldownBar;

        protected bool _IsFlagship = false;

        protected float Health = 100;
        protected int Level = 0;
        protected List<StatusEffect> AppliedStatusEffects = new List<StatusEffect>();
        
        protected float AttackCooldownTimer = 5;
        protected float _HealthBarFillAmount = 1;
        protected float _CooldownBarFillAmount = 1;
        protected int _raycastMask = 1 << 8;
        public int RaycastMask
        {
            get => _raycastMask;
            set => _raycastMask = value;
        }

        protected bool MainCooldownTriggered = false;

        protected Vector3 Heading = new Vector3(0, 1, 0);
        
        public abstract void Damage(DamageInfo damageInfo);
        protected abstract void CalculateMovement();
        protected abstract void OnSubCooldown();
        protected abstract void OnCooldown();
        protected abstract IEnumerator SubCooldownRoutine();
        protected abstract IEnumerator CooldownRoutine();
        
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
    }
    
    public abstract class BaseShip<TData> : BaseShip where TData : BaseShipData
    {
        [SerializeField]
        public TData ShipData;
        
        // Start is called before the first frame update
        public virtual void Start()
        {
            Fleet.Instance.AddToFleet(this);
            Health = ShipData.MaxHealth[Level];
            Level = ShipData.Level;
            AttackCooldownTimer = ShipData.AttackCooldown[Level];
            
            StartCoroutine(CooldownRoutine());
        }

        // Update is called once per frame
        public virtual void Update()
        {
            HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, _HealthBarFillAmount, Time.deltaTime * 10);
            CooldownBar.fillAmount = Mathf.Lerp(CooldownBar.fillAmount, _CooldownBarFillAmount, Time.deltaTime * 10);

            CalculateMovement();
        }

        protected override void CalculateMovement()
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
                        _raycastMask)) continue;
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

        protected override void OnCooldown()
        {
            AttackCooldownTimer = ShipData.AttackCooldown[Level];
            MainCooldownTriggered = true;
            _CooldownBarFillAmount = 0;       
            StartCoroutine(SubCooldownRoutine());
        }

        protected override void OnSubCooldown() { }
        
        protected override IEnumerator CooldownRoutine()
        {
            DOTween.To(()=> _CooldownBarFillAmount, x=> _CooldownBarFillAmount = x, 1, ShipData.AttackCooldown[Level]);
            yield return new WaitForSeconds(ShipData.AttackCooldown[Level]);
            OnCooldown();
        }

        protected override IEnumerator SubCooldownRoutine()
        {
            for (var i = 0; i < ShipData.SubAttackCount[Level]; i++)
            {
                OnSubCooldown();
                yield return new WaitForSeconds(ShipData.SubAttackCooldown);
            }
            StartCoroutine(CooldownRoutine());
            MainCooldownTriggered = false;
        }

        public override void Damage(DamageInfo damageInfo)
        {
            var damage = damageInfo.Damage;
            damage = Mathf.Max(damage - ShipData.Armor[Level]);
            Health -= Mathf.Max(0, damage);
            _HealthBarFillAmount = Health / ShipData.MaxHealth[Level];

            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
