using _Core._6_Enemies.ScriptableObjects;
using UnityEngine;

namespace _Core._6_Enemies
{
    public class EnemyCombat : MonoBehaviour
    {
        #region Movement vars

        [Header("Movement")]
        [SerializeField] private Vector2 knockbackStrength;

        #endregion

        #region components

        [Header("Components")] 
        private Rigidbody2D rb;
        private IEnemy enemy;
        [SerializeField] private EnemyData enemyData;

        #endregion

        #region Combat Vars

        private float enemyHealth;

        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            enemy = (IEnemy)GetComponent(typeof(IEnemy));
            enemyHealth = enemyData.enemyMaxHealth;
        }

        private void Update()
        {
            Death();
        }

        private void Death()
        {
            if (enemyHealth <= 0)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        public void OnDamageTaken(float damage)
        {
            TakeDamage(damage);
            TakeKnockback();
        }

        private void TakeDamage(float damage)
        {
            enemyHealth -= damage;
        }

        private void TakeKnockback()
        {
            var direction = enemy.isFacingRight ? Vector2.left : Vector2.right;
            
            rb.velocity = Vector3.zero;
            rb.AddForce(knockbackStrength.x * 1000 * direction, ForceMode2D.Force);
            rb.AddForce(knockbackStrength.y * 1000 * Vector2.up, ForceMode2D.Force);
        }
    }
}