using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAsteroid : BaseAsteroid
{
    public float Magnetism = 300;
    public Color ColorA;
    public Color ColorB;
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

        ColorTime += Time.deltaTime * 2;
        Color c = Color.Lerp(ColorA, ColorB, Mathf.Cos(ColorTime) * 0.5f + 0.5f);
        m_Renderer.material.SetColor("_Color", c);
    }

    public override void CalculateMovement()
    {
        base.CalculateMovement();

        BaseShip closestShip = Fleet.Instance.GetClosestShip(transform.position);
        if (closestShip != null)
        {
            Vector3 vectorToClosestShip = closestShip.transform.position - transform.position;
            asteroidRigidbody.AddForce(vectorToClosestShip.normalized * Time.deltaTime * Magnetism * (asteroidRigidbody.mass / vectorToClosestShip.magnitude));
        }
    }

    public override BaseAsteroid GenerateChild()
    {
        return AsteroidPrefabManager.InstantiateMagneticAsteroid(this);
    }
}
