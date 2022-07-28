using UnityEngine;

namespace Combat.Asteroid
{
    public class AsteroidPrefabManager : MonoBehaviour
    {
        public DamageIndicator DamageIndicatorPrefab;
        // Asteroid prefabs
        public BaseAsteroid BaseAsteroidPrefab;
        public MagneticAsteroid MagneticAsteroidPrefab;

        // Instance of prefab manager
        public static AsteroidPrefabManager Instance;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        }

        public static void CreateDamageIndicator(float damage, Vector3 position)
        {
            var dmg = Instantiate(Instance.DamageIndicatorPrefab, position, Quaternion.identity);
            dmg.SetText(damage.ToString());
        }

        // Applies a small initial force to asteroid
        private static void ApplyInitialForce(BaseAsteroid Asteroid)
        {
            var Force = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10;
            var Offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            Asteroid.asteroidRigidbody.AddForceAtPosition(Force, Asteroid.transform.position + Offset);
        }

        // Instantiate a new base asteroid based on the input asteroid
        public static BaseAsteroid InstantiateBaseAsteroid(BaseAsteroid ParentAsteroid)
        {
            var NewAsteroid = Instantiate(Instance.BaseAsteroidPrefab, ParentAsteroid.transform.position, ParentAsteroid.transform.rotation);
            NewAsteroid.Size = ParentAsteroid.Size - 1;
            NewAsteroid.asteroidRigidbody.velocity = ParentAsteroid.asteroidRigidbody.velocity;
            NewAsteroid.asteroidRigidbody.angularVelocity = ParentAsteroid.asteroidRigidbody.angularVelocity;
            NewAsteroid.enabled = true;

            ApplyInitialForce(NewAsteroid);

            return NewAsteroid;
        }

        // Instantiate a new base asteroid with given values
        public static BaseAsteroid InstantiateBaseAsteroid(int Size, Vector3 Position, Quaternion Rotation)
        {
            var NewAsteroid = Instantiate(Instance.BaseAsteroidPrefab, Position, Rotation);
            NewAsteroid.Size = Size;
            NewAsteroid.enabled = true;

            ApplyInitialForce(NewAsteroid);

            return NewAsteroid;
        }

        // Instantiate a new magnetic asteroid based on the input asteroid
        public static MagneticAsteroid InstantiateMagneticAsteroid(MagneticAsteroid ParentAsteroid)
        {
            var positionOffset = new Vector3(Random.Range(-0.5f, -0.5f), Random.Range(-0.5f, -0.5f), 0f) * (ParentAsteroid.Size - 1f) / 3f;
            var NewAsteroid = Instantiate(Instance.MagneticAsteroidPrefab, ParentAsteroid.transform.position + positionOffset, ParentAsteroid.transform.rotation);
            NewAsteroid.Size = ParentAsteroid.Size - 1;
            NewAsteroid.asteroidRigidbody.velocity = ParentAsteroid.asteroidRigidbody.velocity;
            NewAsteroid.asteroidRigidbody.angularVelocity = ParentAsteroid.asteroidRigidbody.angularVelocity;
            NewAsteroid.enabled = true;

            ApplyInitialForce(NewAsteroid);

            return NewAsteroid;
        }

        // Instantiate a new magnetic asteroid with given values
        public static MagneticAsteroid InstantiateMagneticAsteroid(int Size, Vector3 Position, Quaternion Rotation)
        {
            var NewAsteroid = Instantiate(Instance.MagneticAsteroidPrefab, Position, Rotation);
            NewAsteroid.Size = Size;
            NewAsteroid.enabled = true;

            ApplyInitialForce(NewAsteroid);

            return NewAsteroid;
        }
    }
}
