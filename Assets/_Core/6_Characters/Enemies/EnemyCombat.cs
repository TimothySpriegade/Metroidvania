using _Core._6_Characters.Enemies.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies
{
    public class EnemyCombat : Destructible
    {
        #region components

        [Header("Components")] 
        private Rigidbody2D rb;
        private IEnemy enemy;
        public EnemyData enemyData { get; private set; }
        private Tween knockbackTween;
        private Tween deathTween;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
            enemy = GetComponent<IEnemy>();
            enemyData = (EnemyData) data;
        }

        public override void OnAttackHit(int damage, GameObject attacker)
        {
            TakeKnockback();
            base.OnAttackHit(damage, attacker);
        }

        protected override void Destroy()
        {
            knockbackTween?.Kill();
            deathTween = DOVirtual.DelayedCall(enemy.DeathAnimation(), base.Destroy);
        }

        private void TakeKnockback()
        {
            enemy.duringAnimation = true;
            var direction = enemy.isFacingRight ? -1 : 1;
            var force = new Vector2(enemyData.knockback.x * direction, enemyData.knockback.y);
            
            rb.velocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);

            knockbackTween = DOVirtual.DelayedCall(0.25f, () => enemy.duringAnimation = false );
        }

        private void OnDisable()
        {
            knockbackTween?.Kill();
            deathTween?.Kill();
        }
    }
}