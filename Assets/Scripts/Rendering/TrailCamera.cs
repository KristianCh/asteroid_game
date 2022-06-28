using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCamera : MonoBehaviour
{
    public RenderTexture TrailRT;
    public RenderTexture TrailHalfRT;
    public RenderTexture TrailQuarterRT;
    public Material TrailPingPongMat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TrailPingPongMat.SetFloat("Time", Time.deltaTime);
        Graphics.Blit(TrailRT, TrailHalfRT, TrailPingPongMat);
        Graphics.Blit(TrailHalfRT, TrailQuarterRT, TrailPingPongMat);
        Graphics.Blit(TrailQuarterRT, TrailHalfRT, TrailPingPongMat);
        Graphics.Blit(TrailHalfRT, TrailRT, TrailPingPongMat);
    }
}
