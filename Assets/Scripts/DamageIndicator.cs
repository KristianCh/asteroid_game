using System;
using System.Drawing;
using System.Numerics;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _Heading = new Vector3(0, 1, 0);
    
    [SerializeField]
    private float _Speed = 100;
    
    [SerializeField]
    private float _Lifetime = 0.5f;
    
    [SerializeField]
    private TMP_Text _DamageText;

    [SerializeField] 
    private float _SpeedDelta = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-45f, 45f));
        _Heading = rotation * _Heading;
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _Heading * _Speed * Time.deltaTime;
        _Speed = Mathf.Lerp(_Speed, 0, Time.deltaTime * _SpeedDelta);
        _Lifetime -= Time.deltaTime;
        if (_Lifetime <= 0)
        {
            // TODO: Pooling
            Destroy(gameObject);
        }
    }

    public void SetVisuals(string text, Color color)
    {
        _DamageText.text = text;
        _DamageText.color = color;
    }
}
