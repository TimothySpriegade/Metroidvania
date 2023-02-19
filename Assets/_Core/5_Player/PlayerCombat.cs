using DG.Tweening;
using SOEventSystem.Events;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerCombat : MonoBehaviour
    {
        #region vars

        #region PlayerCombat Vars

        [Header("Combat")] 
        [SerializeField] private GameObject attackArea;
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
            // Preparation
            LastPressedAttackTime = 0;
            
            // Animation
            var attackLength = animator.AttackAnimation();
            
            // Activating attack hitbox
            attackArea.SetActive(true);
            lastAttackedTime = attackLength;

            // Deactivating attack hitbox
            DOVirtual.DelayedCall(attackLength, () => attackArea.SetActive(false));
        }
    }
}
