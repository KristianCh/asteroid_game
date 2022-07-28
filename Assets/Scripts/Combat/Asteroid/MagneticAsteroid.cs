using Combat.Ships;
using UnityEngine;

namespace Combat.Asteroid
{
    public class MagneticAsteroid : BaseAsteroid
    {
        // Strength of magnetic force
        public float Magnetism = 500;
        // Colors to use
        public Color ColorA;
        public Color ColorB;
        // Time to base color of
        private float ColorTime = 0f;


        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            // Update asteroid color
            ColorTime += Time.deltaTime * 2;
            Color c = Color.Lerp(ColorA, ColorB, Mathf.Cos(ColorTime) * 0.5f + 0.5f);
            m_Renderer.material.SetColor("_Color", c);
        }

        // Base asteroid movement + adding force towards closest ship
        public override void CalculateMovement()
        {
            base.CalculateMovement();

            if (Fleet.Instance == null)
            {
                return;
            }
            var closestShip = Fleet.Instance.GetClosestShip(transform.position);
            if (closestShip != null)
            {
                Vector3 vectorToClosestShip = closestShip.transform.position - transform.position;
                asteroidRigidbody.AddForce(vectorToClosestShip.normalized * Time.deltaTime * Magnetism * (asteroidRigidbody.mass / vectorToClosestShip.magnitude));
            }
        }

        // Generate magnetic asteroid instead
        public override BaseAsteroid GenerateChild()
        {
            return AsteroidPrefabManager.InstantiateMagneticAsteroid(this);
        }
    }
}
