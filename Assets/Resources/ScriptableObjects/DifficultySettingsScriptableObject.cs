using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "Difficulty")]
public class DifficultySettingsScriptableObject : ScriptableObject
{
    public string Title;
    [TextArea(15, 15)]
    public string Description;

    public float RepairPercentage;
    public float AsteroidSpawnRate;
    public float AsteroidToughnessModifier;
    public float AsteroidToughnessMultiplier;
    public float CreditGainMultiplier;

    public int[] BossStages;
    
    public bool DoDestroyShips;
    public bool SpawnAllAtFirstStage;
}
