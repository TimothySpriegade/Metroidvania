using _Core._6_Characters.Enemies;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerCombatHandler : MonoBehaviour
    {
        #region vars

        #region Health Vars

        #endregion

        #region Data


        private PlayerMovement player;

        #endregion

        #region Components

        private Rigidbody2D rb;

        #endregion

        #endregion

        #region UnityMethods

        private void Awake()
        {
            player = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody2D>();

            HealthSpriteFinder();
        }

        

        #endregion

        #region healthMethods


        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (invincibilityTime <= 0 || !collision.gameObject.CompareTag("Enemy")) return;
            
            invincibilityTime = 0.5f;
            var enemyDamage = collision.gameObject.GetComponentInParent<EnemyCombat>().enemyData.damage;
            TakeDamage(enemyDamage);
        }

        

        
    }
}