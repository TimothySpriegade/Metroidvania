using _Core._6_Characters.Enemies.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies
{
    [RequireComponent(typeof(IEnemy))]
    public class EnemyCombat : Destructible
    {
        #region components

        [Header("Components")] 
        private Rigidbody2D rb;
        private IEnemy enemy;
        private EnemyData enemyData;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
            enemy = GetComponent<IEnemy>();
            enemyData = (EnemyData) data;
        }

        public override void OnAttackHit(float damage)
        {
            base.OnAttackHit(damage);
            TakeKnockback();
        }

        protected override void Destroy()
        {
            DOVirtual.DelayedCall(enemy.DeathAnimation(), base.Destroy);
        }

        private void TakeKnockback()
        {
            enemy.duringAnimation = true;
            var direction = enemy.isFacingRight ? -1 : 1;
            var force = new Vector2(enemyData.knockback.x * direction, enemyData.knockback.y);
            
            rb.velocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);

            DOVirtual.DelayedCall(0.5f, () => enemy.duringAnimation = false);
        }
    }
}