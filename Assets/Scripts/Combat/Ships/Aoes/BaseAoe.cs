using DG.Tweening;
using UnityEngine;

namespace Combat.Ships.Aoes
{
    public enum AoeShape
    {
        Circle,
        Square
    }

    public enum AoeType
    {
        Explosion
    }

    public class BaseAoe : ShipSpawnableObject
    {
        public AoeShape Shape;
        public AoeType Type;
        public float Lifetime = 1;
        public float Scale = 1;

        private float Angle = 0;
    
        private const float DEATH_TIME = 1f;
        private const float START_TIME = 0.2f;
    
        public virtual void Start()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Scale, START_TIME);
        
            if (Lifetime > DEATH_TIME)
            {
                Utilities.Utility.InvokeAfter(this, StartDeath, Lifetime - DEATH_TIME);  
            }
            else
            {
                StartDeath();
            }
        
        }
    
        public virtual void Update()
        {
            Lifetime -= Time.deltaTime;
            Angle += Time.deltaTime * 360 * 2;
            transform.rotation = Quaternion.Euler(0, 0, Angle);
        }

        private void StartDeath()
        {
            transform.DOScale(Vector3.zero, Mathf.Min(DEATH_TIME - START_TIME, Lifetime)).SetDelay(START_TIME);
            Utilities.Utility.InvokeAfter(this, Death, Mathf.Min(DEATH_TIME, Lifetime));
        }

        protected virtual void Death()
        {
            Destroy(gameObject);
        }
    }
}