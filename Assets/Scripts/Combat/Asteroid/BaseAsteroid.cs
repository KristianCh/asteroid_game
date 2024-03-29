using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.Asteroid
{
    public enum AsteroidType
    {
        Normal,
        Magnetic
    }

    public class BaseAsteroid : MonoBehaviour, IDamagable
    {
        // base stats
        public float GravityPull = 10;
        public float Health = 100;
        public float BaseMaxHealth = 100;
        public int Size = 3;
        public bool IsDead = false;
        public Dictionary<string, float> DamageTypeMultipliers = new Dictionary<string, float>();

        // Components
        public Rigidbody asteroidRigidbody;
        public ParticleSystem DamageParticles;

        public Image HealthBar;
        public Image OutOfVisionMarker;

        private SphereCollider m_Collider;
        private MeshGenerator m_MeshGenerator;

        public Renderer m_Renderer;
        public Canvas ScreenCanvas;

        // Timers to use for offset of gravity center and time between "death" and actually destroying go
        private float _centerOffset = 0;
        private float _deathTime = 1;
        // Velocity magnitude to go to when out of screen
        private readonly float OutOfScreenSpeedMax = 5;
        
        // Set X position of marker
        private const int ScreenX = 960; //(Screen.width / 2);
        private const int ScreenY = 540; //(Screen.height / 2);
        private const int BorderW = 20; //Screen.height / 1080 * 20;

        // Start is called before the first frame update
        public virtual void Start()
        {
            // Add to game manager list of asteroids
            if (GameManager.Instance != null)
            {
                GameManager.AddAsteroid(this);
            }

            // set max health according to asteroid size
            BaseMaxHealth = BaseMaxHealth * Size;
            Health = BaseMaxHealth;

            // Get components
            _centerOffset = Random.Range(0, 6);
            asteroidRigidbody = GetComponent<Rigidbody>();
            m_MeshGenerator = GetComponent<MeshGenerator>();
            m_Collider = GetComponent<SphereCollider>();

            // Scale particle fx and healthbar
            asteroidRigidbody.mass = Mathf.Pow(2, Size - 1);
            ParticleSystem.ShapeModule ps = DamageParticles.shape;
            ps.radius = Size / 3.0f;
            HealthBar.transform.localScale = new Vector3(ps.radius, ps.radius, ps.radius);

            // Generate mesh
            m_MeshGenerator.Size = Size / 6.0f;
            m_Collider.radius = m_MeshGenerator.Size;
            if (Size == 1)
            {
                m_MeshGenerator.Subdivisions = 0;
            }

            m_MeshGenerator.Subdivisions = 1;
            m_MeshGenerator.Create();

            // Set healthbar canvas camera
            ScreenCanvas.worldCamera = Camera.main;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            // Reduce timer and scale
            if (IsDead)
            {
                _deathTime -= Time.deltaTime;
                transform.localScale = new Vector3(_deathTime, _deathTime, _deathTime);

                if (_deathTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
            // Obvious
            CalculateMovement();
            UpdateOutOfVisionMarker();
        }

        // Finalize what needs to be done
        public virtual void Death()
        {
            /* 
         * TODO : on death effects
         */

            GameManager.RemoveAsteroid(this);
            OutOfVisionMarker.enabled = false;

            IsDead = true;
            Health = 0;
            if (Size > 1)
            {
                for (var i = 0; i < Random.Range(1, 3); i++)
                {
                    GenerateChild();
                }
            }
        }

        // Handle collision
        public virtual void OnCollisionEnter(Collision collision)
        {
            // Ignore if dead
            if (IsDead) return;
        
            // Get first contact point (There shouldn't be more anyway)
            ContactPoint contact = collision.contacts[0];
        
            // Get rigidbody of colliding object and its mass
            var MassMult = 1f;
            var OtherRigidbody = collision.body as Rigidbody;
            if (OtherRigidbody)
            {
                MassMult = OtherRigidbody.mass;
            }

            // Add force to knock asteroid away from coliding object
            asteroidRigidbody.AddForceAtPosition(collision.impulse * Mathf.Max(20, Mathf.Min(collision.relativeVelocity.magnitude * 80, 50)) * MassMult, 
                contact.point);

            // Calculate and apply damage dealt to asteroid
            var damage = Mathf.Pow(1 + collision.relativeVelocity.magnitude, 2) * MassMult;
            Damage(
                new DamageInfo(damage, DamageType.Kinetic, contact.point, collision.gameObject.layer != LayerMask.NameToLayer("ShipProjectile"))
            );
        }

        // Calculates forces applied to asteroid each frame
        public virtual void CalculateMovement()
        {
            // Calculate position of gravity
            _centerOffset += Time.deltaTime;
            Vector3 GravityPosition = new Vector3(Mathf.Cos(_centerOffset), Mathf.Sin(_centerOffset), 0) * 0.2f;
            // Apply force to move asteroid towards the offset center of the world
            asteroidRigidbody.AddForce(GravityPosition - transform.position * Time.deltaTime * GravityPull);

            // If the asteroid is out of the cameras vision, slow it down if it's too fast
            if (
                (Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect ||
                 Mathf.Abs(transform.position.y) > Camera.main.orthographicSize) &&
                asteroidRigidbody.velocity.magnitude > OutOfScreenSpeedMax
            )
            {
                asteroidRigidbody.AddForce(-asteroidRigidbody.velocity.normalized * Time.deltaTime * 2f * (transform.position.magnitude));
            }
        }

        // Apply damage to asteroid
        public virtual void Damage(DamageInfo damageInfo)
        {
            /*
         * Todo : Take resistances and weaknesses into account
         */
            var damage = damageInfo.Damage;
            if (IsDead) return;
            DamageParticles.Play(true);
            Health -= damage;
            if (Health <= 0)
            {
                Death();
            }
            if (damageInfo.ShowDamageIndicator) AsteroidPrefabManager.CreateDamageIndicator(Mathf.Ceil(damage), damageInfo);
            HealthBar.fillAmount = Health / BaseMaxHealth;
        }

        // Calculates and sets the position of the arrow indicating the location of the asteroid if it's out of cameras vision
        private void UpdateOutOfVisionMarker()
        {
            // Calculate bounds of vision
            var lowerXBound = -Camera.main.orthographicSize * Camera.main.aspect + Camera.main.transform.position.x;
            var upperXBound = Camera.main.orthographicSize * Camera.main.aspect + Camera.main.transform.position.x;

            var lowerYBound = -Camera.main.orthographicSize + Camera.main.transform.position.y;
            var upperYBound = Camera.main.orthographicSize + Camera.main.transform.position.y;

            // Check what bounds are OK
            var isInNegXRange = transform.position.x >= lowerXBound;
            var isInPosXRange = transform.position.x <= upperXBound;

            var isInNegYRange = transform.position.y >= lowerYBound;
            var isInPosYRange = transform.position.y <= upperYBound;

            // End if asteroid is in cameras vision and set the markers color appropriately
            if (isInNegXRange && isInPosXRange && isInNegYRange && isInPosYRange)
            {
                OutOfVisionMarker.color = new Color(0, 0, 0, 0);
                return;
            }
            else
            {
                OutOfVisionMarker.color = m_Renderer.material.color;
            }

            // Initialize marker position and calculate screen space coords of asteroid
            var markerPosition = new Vector2(0, 0);
            var ScreenSpacePos = new Vector2(
                (transform.position.x - Camera.main.transform.position.x) / (Camera.main.orthographicSize * Camera.main.aspect) * (Screen.width / 2),
                (transform.position.y - Camera.main.transform.position.y) / Camera.main.orthographicSize * (Screen.height / 2)
            );

            
            if (!isInNegXRange)
            {
                markerPosition.x = -Camera.main.orthographicSize * Camera.main.aspect / (Camera.main.orthographicSize * Camera.main.aspect) * ScreenX + BorderW;
            }
            if (!isInPosXRange)
            {
                markerPosition.x = Camera.main.orthographicSize * Camera.main.aspect / (Camera.main.orthographicSize * Camera.main.aspect) * ScreenX - BorderW;
            }
            if (isInNegXRange && isInPosXRange)
            {
                markerPosition.x = ScreenSpacePos.x;
                markerPosition.x = Mathf.Max(-(ScreenX - BorderW), markerPosition.x);
                markerPosition.x = Mathf.Min(ScreenX - BorderW, markerPosition.x);
            }

            // Set Y position of marker
            if (!isInNegYRange)
            {
                markerPosition.y = -Camera.main.orthographicSize / Camera.main.orthographicSize * ScreenY + BorderW;
            }
            if (!isInPosYRange)
            {
                markerPosition.y = Camera.main.orthographicSize / Camera.main.orthographicSize * ScreenY - BorderW;
            }
            if (isInNegYRange && isInPosYRange)
            {
                markerPosition.y = ScreenSpacePos.y;
                markerPosition.y = Mathf.Max(-(ScreenY - BorderW), markerPosition.y);
                markerPosition.y = Mathf.Min(ScreenY - BorderW, markerPosition.y);
            }

            // Apply marker position
            var markerTransform = OutOfVisionMarker.GetComponent<RectTransform>();
            markerTransform.anchoredPosition = markerPosition;

            // Calculcate and set angle and scale of marker
            var angle = Mathf.Atan2(markerPosition.y, markerPosition.x) * Mathf.Rad2Deg - 90;
            var scaleFactor = Mathf.Min(2.0f, (ScreenSpacePos - markerPosition).magnitude / 500.0f);

            markerTransform.eulerAngles = new Vector3(0, 0, angle);
            markerTransform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        // Generate an asteroid with this asteroid as it's "parent" (Not parent game object, use this asteroid to base its position and stats off of)
        protected virtual BaseAsteroid GenerateChild()
        {
            return AsteroidPrefabManager.InstantiateBaseAsteroid(this);
        }

    }
}