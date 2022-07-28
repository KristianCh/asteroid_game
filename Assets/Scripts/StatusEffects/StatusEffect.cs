using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public bool OneTime = true;
    public bool Timed = false;
    public float Lifetime = 0;
    private BaseShip Ship;

    public bool Apply(BaseShip ship)
    {
        Ship = ship;
        return true;
    }

    public IEnumerable Remove()
    {
        yield return null;
    }
}
