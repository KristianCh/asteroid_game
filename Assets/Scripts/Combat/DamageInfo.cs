using UnityEngine;

namespace Combat
{
    public class DamageInfo
    {
        public float Damage = 0;
        public DamageType Type = DamageType.Kinetic;
        public Vector3 Position = Vector3.zero;
        public bool ShowDamageIndicator = false;

        public DamageInfo
        (
            float damage,
            DamageType damageType,
            Vector3 position,
            bool showDamageIndicator
        )
        {
            Damage = damage;
            Type = damageType;
            Position = position;
            ShowDamageIndicator = showDamageIndicator;
        }
    }
}