using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public Vector3 Heading = new Vector3(0, 1, 0);
    public float Speed = 100;
    public float Lifetime = 0.5f;

    public TMP_Text DamageText;

    // Start is called before the first frame update
    void Start()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-45f, 45f));
        Heading = rotation * Heading;
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Heading * Speed * Time.deltaTime;
        Speed = Mathf.Lerp(Speed, 0, Time.deltaTime * 2);
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetVisuals(string text, Color color)
    {
        DamageText.text = text;
        DamageText.color = color;
    }
}
