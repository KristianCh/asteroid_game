using System;
using System.Numerics;
using UnityEngine;

namespace Combat.Asteroid
{
    public class AsteroidPrefabManager : MonoBehaviour
    {
        public static AsteroidPrefabManager Instance;
        
        [SerializeField]
        private DamageIndicator _DamageIndicatorPrefab;
        // Asteroid prefabs
        
        [SerializeField]
        private BaseAsteroid BaseAsteroidPrefab;
        
        [SerializeField]
        private MagneticAsteroid MagneticAsteroidPrefab;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        }

        // TODO: pooling, implement initial setup in classes themselves
        
        public static void CreateDamageIndicator(float damage, DamageInfo damageInfo)
        {
            var dmg = Instantiate(Instance._DamageIndicatorPrefab, damageInfo.Position, Quaternion.identity);
            dmg.SetVisuals(damage.ToString(), damageInfo.DamageColor);
        }

        // Applies a small initial force to asteroid
        private static void ApplyInitialForce(BaseAsteroid asteroid)
        {
            var force = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10;
            var offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            asteroid.asteroidRigidbody.AddForceAtPosition(force, asteroid.transform.position + offset);
        }

        // Instantiate a new base asteroid based on the input asteroid
        public static BaseAsteroid InstantiateBaseAsteroid(BaseAsteroid parentAsteroid)
        {
            var newAsteroid = Instantiate(Instance.BaseAsteroidPrefab, parentAsteroid.transform.position, parentAsteroid.transform.rotation);
            newAsteroid.Size = parentAsteroid.Size - 1;
            newAsteroid.asteroidRigidbody.velocity = parentAsteroid.asteroidRigidbody.velocity;
            newAsteroid.asteroidRigidbody.angularVelocity = parentAsteroid.asteroidRigidbody.angularVelocity;
            newAsteroid.enabled = true;

            ApplyInitialForce(newAsteroid);

            return newAsteroid;
        }

        // Instantiate a new base asteroid with given values
        public static BaseAsteroid InstantiateBaseAsteroid(int size, Vector3 position, Quaternion rotation)
        {
            var newAsteroid = Instantiate(Instance.BaseAsteroidPrefab, position, rotation);
            newAsteroid.Size = size;
            newAsteroid.enabled = true;

            ApplyInitialForce(newAsteroid);

            return newAsteroid;
        }

        // Instantiate a new magnetic asteroid based on the input asteroid
        public static MagneticAsteroid InstantiateMagneticAsteroid(MagneticAsteroid parentAsteroid)
        {
            var positionOffset = new Vector3(Random.Range(-0.5f, -0.5f), Random.Range(-0.5f, -0.5f), 0f) * (parentAsteroid.Size - 1f) / 3f;
            var newAsteroid = Instantiate(Instance.MagneticAsteroidPrefab, parentAsteroid.transform.position + positionOffset, parentAsteroid.transform.rotation);
            newAsteroid.Size = parentAsteroid.Size - 1;
            newAsteroid.asteroidRigidbody.velocity = parentAsteroid.asteroidRigidbody.velocity;
            newAsteroid.asteroidRigidbody.angularVelocity = parentAsteroid.asteroidRigidbody.angularVelocity;
            newAsteroid.enabled = true;

            ApplyInitialForce(newAsteroid);

            return newAsteroid;
        }

        // Instantiate a new magnetic asteroid with given values
        public static MagneticAsteroid InstantiateMagneticAsteroid(int size, Vector3 position, Quaternion rotation)
        {
            var newAsteroid = Instantiate(Instance.MagneticAsteroidPrefab, position, rotation);
            newAsteroid.Size = size;
            newAsteroid.enabled = true;

            ApplyInitialForce(newAsteroid);

            return newAsteroid;
        }
    }
}
