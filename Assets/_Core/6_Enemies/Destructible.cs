using UnityEngine;

namespace _Core._6_Enemies
{
    public abstract class Destructible : MonoBehaviour
    {
        protected float health;

        private void Death()
        {
            Destroy(transform.parent.gameObject);
        }

        public virtual void OnDamageTaken(float damage)
        {
            TakeDamage(damage);

            if (health <= 0)
            {
                Death();
            }
        }

        private void TakeDamage(float damage)
        {
            health -= damage;
        }

        
    }
}
