using _Framework;
using UnityEngine;

namespace _Core._6_Characters.Enemies
{
    public abstract class Destructible : MonoBehaviour
    {
        protected float health;

        protected virtual void Destroy()
        {
            this.Log("HP was reduced to 0, destroying destructible.");
            Destroy(transform.parent.gameObject);
        }

        public virtual void OnDamageTaken(float damage)
        {
            TakeDamage(damage);

            if (health <= 0)
            {
                Destroy();
            }
        }

        private void TakeDamage(float damage)
        {
            health -= damage;
        }
    }
}