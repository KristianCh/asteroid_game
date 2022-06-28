using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public bool OneTime = true;
    public bool Timed = false;
    public float Lifetime = 0;

    public bool Apply(BaseShip Ship)
    {
        return true;
    }
}
