using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AoeShape
{
    Circle,
    Square
}

public enum AoeType
{
    Explosion
}

public class BaseAoe : MonoBehaviour
{
    public AoeShape Shape;
    public AoeType Type;
    public float Lifetime = 1;
    public float Scale = 1;
    private Vector3 TargetScale = new Vector3(1, 1, 1);

    private float Angle = 0;

    // Start is called before the first frame update
    public virtual void Start()
    {
        transform.localScale = Vector3.zero;
        SetScale(Scale);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Lifetime -= Time.deltaTime;
        Angle += Time.deltaTime * 360 * 2;
        transform.rotation = Quaternion.Euler(0, 0, Angle);

        transform.localScale = Vector3.Lerp(transform.localScale, TargetScale, Time.deltaTime * 10);

        if (Lifetime <= 1)
        {
            TargetScale = new Vector3(Scale, Scale, Scale) * Lifetime;
        }

        if (Lifetime <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public void SetScale(float scale)
    {
        Scale = scale;
        TargetScale = new Vector3(Scale, Scale, Scale);
    }
}
