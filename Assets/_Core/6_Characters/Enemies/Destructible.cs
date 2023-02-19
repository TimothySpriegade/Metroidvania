using _Framework;
using UnityEngine;

namespace _Core._6_Enemies
{
    public abstract class Destructible : MonoBehaviour
    {
        protected float health;

        protected virtual void Death()
        {
            this.Log("HP was reduced to 0, destroying enemy.");
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
