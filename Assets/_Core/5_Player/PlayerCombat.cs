using Player;
using SOEventSystem.Events;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerCombat : MonoBehaviour
    {
        #region vars

        #region playercombatvars

        [Header("Combat")]
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float playerDamage;
        public float LastPressedAttackTime { get; set; }
        private float lastAttackedTime;

        #endregion

        #region events

        [Header("Events")]
        [SerializeField] private FloatEvent onDamageGiven;

        #endregion

        #region components
        
        private PlayerAnimator animator;
        
        #endregion

        #endregion

        #region UnityMethods

        private void Awake()
        {
            animator = GetComponent<PlayerAnimator>();
        }
        private void Update()
        {
            LastPressedAttackTime -= Time.deltaTime;
            lastAttackedTime -= Time.deltaTime;
        
            if (LastPressedAttackTime > 0 && CanAttack())
            {
                Attack();
            }
        }

        #endregion

        private bool CanAttack()
        {
            return lastAttackedTime <= 0; // TODO add conditions
        }

        private void Attack()
        {
            LastPressedAttackTime = 0;
            lastAttackedTime = attackCooldown;
            animator.ChangeAnimationState(PlayerAnimatorState.PlayerAttack);
            var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach(var enemy in hitEnemies) 
            {
                onDamageGiven.Invoke(playerDamage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
