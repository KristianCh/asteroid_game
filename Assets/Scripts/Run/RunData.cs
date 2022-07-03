using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ShipEntry
{
    public string Type = "";
    public string Name = "";
    public float HealthPercentage = 100;
    public int Level = 1;
    public int SubLevel = 0;
    public bool IsFlagShip = false;

    public ShipEntry(string type, float healthPercentage, int level, int subLevel, bool isFlagship)
    {
        Type = type;
        HealthPercentage = healthPercentage;
        Level = level;
        SubLevel = subLevel;
        IsFlagShip = isFlagship;
    }
}

public class RunData : MonoBehaviour
{
    public List<ShipEntry> Ships = new List<ShipEntry>();
    public List<Module> Modules = new List<Module>();
    public int Stage = 1;
    public int Credits = 0;
    public int FleetLimit => 3 + Stage / 3;
    public GameDifficulty Difficulty = GameDifficulty.Normal;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
