using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWaveManager : WaveManager
{
    // Start is called before the first frame update
    public override void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
