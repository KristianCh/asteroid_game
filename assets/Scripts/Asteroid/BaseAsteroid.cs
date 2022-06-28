using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAsteroid : MonoBehaviour
{
    public float GravityPull = 10;
    public float Health = 100;
    public float BaseMaxHealth = 100;
    public int Size = 3;
    public bool IsDead = false;

    public Rigidbody asteroidRigidbody;
    public ParticleSystem DamageParticles;
    public Dictionary<string, float> Resistances = new Dictionary<string, float>();

    public Image HealthBar;

    private SphereCollider m_Collider;
    private MeshGenerator m_MeshGenerator;
    private float CenterOffset = 0;
    private float DeathTime = 1;


    // Start is called before the first frame update
    public virtual void Start()
    {
        WaveManager.Instance.ActiveAsteroids.Add(this);

        BaseMaxHealth = 100 * (Size / 3.0f);
        Health = BaseMaxHealth;

        CenterOffset = Random.Range(0, 6);
        asteroidRigidbody = GetComponent<Rigidbody>();
        m_MeshGenerator = GetComponent<MeshGenerator>();
        m_Collider = GetComponent<SphereCollider>();

        asteroidRigidbody.mass = Mathf.Pow(2, Size - 1);
        ParticleSystem.ShapeModule ps = DamageParticles.shape;
        ps.radius = Size / 3.0f;
        HealthBar.transform.localScale = new Vector3(ps.radius, ps.radius, ps.radius);

        m_MeshGenerator.Size = Size / 6.0f;
        m_Collider.radius = m_MeshGenerator.Size;
        if (Size == 1)
        {
            m_MeshGenerator.Subdivisions = 0;
        }
        m_MeshGenerator.Create();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (IsDead)
        {
            DeathTime -= Time.deltaTime;
            transform.localScale = new Vector3(DeathTime, DeathTime, DeathTime);

            if (DeathTime <= 0)
            {
                Death();
            }
        }
        CalculateMovement();
    }

    public virtual void Death()
    {
        WaveManager.Instance.ActiveAsteroids.Remove(this);
        Destroy(gameObject);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (IsDead) return;

        float damage = 0;
        foreach (ContactPoint contact in collision.contacts)
        {
            float MassMult = 1;
            Rigidbody OtherRigidbody = collision.body as Rigidbody;
            if (OtherRigidbody)
            {
                MassMult = OtherRigidbody.mass;
            }

            asteroidRigidbody.AddForceAtPosition(collision.impulse.normalized * Mathf.Max(20, Mathf.Min(collision.relativeVelocity.magnitude * 100, 50)) * MassMult, 
                contact.point);

            damage += Mathf.Pow(1 + collision.relativeVelocity.magnitude, 2) * MassMult;
            break;
        }
        Damage(damage, DamageType.Kinetic);
    }

    public virtual void CalculateMovement()
    {
        CenterOffset += Time.deltaTime;
        Vector3 GravityPosition = new Vector3(Mathf.Cos(CenterOffset), Mathf.Sin(CenterOffset), 0) * 0.2f;
        asteroidRigidbody.AddForce(GravityPosition - transform.position * Time.deltaTime * GravityPull);

        if (
                (Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect ||
                Mathf.Abs(transform.position.y) > Camera.main.orthographicSize) &&
                asteroidRigidbody.velocity.magnitude > 7
            )
        {
            asteroidRigidbody.AddForce(-asteroidRigidbody.velocity.normalized * Time.deltaTime);
        }
    }

    public virtual void Damage(float damage, DamageType damageType)
    {
        if (IsDead) return;
        DamageParticles.Play(true);
        Health -= damage;
        if (Health <= 0)
        {
            IsDead = true;
            Health = 0;
            if (Size > 1)
            {
                for (int i = 0; i < Size; i++)
                {
                    GenerateChild();
                }
            }
        }
        HealthBar.fillAmount = Health / BaseMaxHealth;
    }

    public virtual BaseAsteroid GenerateChild()
    {
        return AsteroidPrefabManager.InstantiateBaseAsteroid(this);
    }
}
