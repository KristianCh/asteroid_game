using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private Material _BackgroundMat;
    
    private float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * 0.5f;
        _BackgroundMat.SetFloat("Time", time);
    }
}
