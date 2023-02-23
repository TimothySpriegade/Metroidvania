using System.Collections;
using _Framework;
using UnityEngine;

namespace _Core._6_Characters.Enemies
{
    public class Destructible : Hittable
    {
        [Header("Destructible")]
        [SerializeField] private GameObject destructibleParent;

        private bool Invincible { get; set; }
        protected float health;
        
        protected override void Awake()
        {
            base.Awake();
            destructibleParent ??= transform.gameObject;
        }

        protected virtual void Destroy()
        {
            this.Log("HP was reduced to 0, destroying destructible.");
            Destroy(transform.gameObject);
        }

        public override void OnAttackHit(float damage)
        {
            // Taking damage
            if (!Invincible) 
                TakeDamage(damage);
            
            // Destroying when health <= 0
            if (health <= 0)
            {
                Destroy();
            }
            
            // Hit effect
            base.OnAttackHit(damage);

            // Starting Iframes
            Invincible = true;
            StartCoroutine(ResetInvincibleFrames(0.15f));
        }

        private IEnumerator ResetInvincibleFrames(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            Invincible = false;
        }

        private void TakeDamage(float damage)
        {
            health -= damage;
        }
        
        
    }
}