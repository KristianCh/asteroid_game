using System.Collections.Generic;
using Combat.Ships;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName="Ship", menuName="Ship")]
    public class BaseShipData : ScriptableObject
    {
        // TODO: use serializefields
        /* Store data */
        public string Type;
        public string Name;
        [TextArea(15, 15)]
        public string Description;
        public int Cost;
        /* General data */
        public float[] MaxHealth;
        public float[] Armor;
        public float Speed;
        public float TurnSpeed;
        public float VisibilityRange;
        public float TargetingRange;
        public int Level;
        public int SubLevel;
        public float[] AttackCooldown;
        public float SubAttackCooldown;
        public int[] SubAttackCount;
        public float[] Damage;
        public float ProjectileSpeed;
        public float ProjectileTracking;
        public int LayerMask = 1 << 8;
        public List<ShipClass> Classes;
        public ShipSpawnableObject SpawnOnAttackPrefab;
        /* Visual data */
        public Mesh m_Mesh;
    }
}
