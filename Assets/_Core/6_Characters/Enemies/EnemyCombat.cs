using _Core._6_Characters.Enemies.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies
{
    public class EnemyCombat : Destructible
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

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            enemy = (IEnemy) GetComponent(typeof(IEnemy));
            health = enemyData.maxHealth;
        }

        public override void OnDamageTaken(float damage)
        {
            base.OnDamageTaken(damage);
            TakeKnockback();
        }

        protected override void Destroy()
        {
            DOVirtual.DelayedCall(enemy.DeathAnimation(), base.Destroy);
        }

        private void TakeKnockback()
        {
            enemy.duringAnimation = true;
            var direction = enemy.isFacingRight ? Vector2.left : Vector2.right;

            rb.velocity = Vector3.zero;
            rb.AddForce(knockbackStrength.x * direction, ForceMode2D.Impulse);
            rb.AddForce(knockbackStrength.y * Vector2.up, ForceMode2D.Impulse);

            DOVirtual.DelayedCall(0.5f, () => enemy.duringAnimation = false);
        }
    }
}