using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPrefabManager : MonoBehaviour
{
    public BaseAsteroid BaseAsteroidPrefab;
    public MagneticAsteroid MagneticAsteroidPrefab;

    public static AsteroidPrefabManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void ApplyInitialForce(BaseAsteroid Asteroid)
    {
        Vector3 Force = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10;
        Vector3 Offset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        Asteroid.asteroidRigidbody.AddForceAtPosition(Force, Asteroid.transform.position + Offset);
    }

    public static BaseAsteroid InstantiateBaseAsteroid(BaseAsteroid ParentAsteroid)
    {
        BaseAsteroid NewAsteroid = Instantiate(Instance.BaseAsteroidPrefab, ParentAsteroid.transform.position, ParentAsteroid.transform.rotation);
        NewAsteroid.Size = ParentAsteroid.Size - 1;
        NewAsteroid.asteroidRigidbody.velocity = ParentAsteroid.asteroidRigidbody.velocity;
        NewAsteroid.asteroidRigidbody.angularVelocity = ParentAsteroid.asteroidRigidbody.angularVelocity;
        NewAsteroid.enabled = true;

        ApplyInitialForce(NewAsteroid);

        return NewAsteroid;
    }

    public static BaseAsteroid InstantiateBaseAsteroid(int Size, Vector3 Position, Quaternion Rotation)
    {
        BaseAsteroid NewAsteroid = Instantiate(Instance.BaseAsteroidPrefab, Position, Rotation);
        NewAsteroid.Size = Size;
        NewAsteroid.enabled = true;

        ApplyInitialForce(NewAsteroid);

        return NewAsteroid;
    }

    public static MagneticAsteroid InstantiateMagneticAsteroid(MagneticAsteroid ParentAsteroid)
    {
        Vector3 positionOffset = new Vector3(Random.Range(-0.5f, -0.5f), Random.Range(-0.5f, -0.5f), 0f) * (ParentAsteroid.Size - 1f) / 3f;
        MagneticAsteroid NewAsteroid = Instantiate(Instance.MagneticAsteroidPrefab, ParentAsteroid.transform.position + positionOffset, ParentAsteroid.transform.rotation);
        NewAsteroid.Size = ParentAsteroid.Size - 1;
        NewAsteroid.asteroidRigidbody.velocity = ParentAsteroid.asteroidRigidbody.velocity;
        NewAsteroid.asteroidRigidbody.angularVelocity = ParentAsteroid.asteroidRigidbody.angularVelocity;
        NewAsteroid.enabled = true;

        ApplyInitialForce(NewAsteroid);

        return NewAsteroid;
    }

    public static MagneticAsteroid InstantiateMagneticAsteroid(int Size, Vector3 Position, Quaternion Rotation)
    {
        MagneticAsteroid NewAsteroid = Instantiate(Instance.MagneticAsteroidPrefab, Position, Rotation);
        NewAsteroid.Size = Size;
        NewAsteroid.enabled = true;

        ApplyInitialForce(NewAsteroid);

        return NewAsteroid;
    }
}
