using _Core._5_Player.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerCombat : MonoBehaviour
    {
        #region vars

        #region PlayerCombat Vars

        [Header("Combat")] 
        [SerializeField] private GameObject attackArea;
        [SerializeField] private Vector2 knockbackStrength;
        public float LastPressedAttackTime { get; set; }
        private float lastAttackedTime;
        private float invincibilityTime;

        #endregion

        #region components
        
        [Header("Components")]
        [SerializeField] private PlayerCombatData data;
        [SerializeField] private IntEvent playerTookDamageEvent;
        
        private PlayerAnimator animator;
        private PlayerMovement movement;
        private Rigidbody2D rb;

        #endregion

        #endregion

        #region UnityMethods

        private void Awake()
        {
            animator = GetComponent<PlayerAnimator>();
            movement = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            invincibilityTime -= Time.deltaTime;
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
        
        
        private void TakeDamage(int amount)
        {
            data.currentHealth -= amount;
            TakeKnockback();
            
            /* if (currentHealth <= 0)
            {
                GameObject.Destroy(gameObject);
            } */

            playerTookDamageEvent.Invoke();
        }
        private void TakeKnockback()
        {
            movement.IgnoreRun = true;

            var direction = movement.IsFacingRight ? Vector2.left : Vector2.right;
            rb.velocity = Vector3.zero;
            rb.AddForce(knockbackStrength.x * direction, ForceMode2D.Impulse);
            rb.AddForce(knockbackStrength.y * Vector2.up, ForceMode2D.Impulse);
            DOVirtual.DelayedCall(0.5f, () => movement.IgnoreRun = false);
        }
    }
}