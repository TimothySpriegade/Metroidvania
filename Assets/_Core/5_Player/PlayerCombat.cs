using _Core._5_Player.ScriptableObjects;
using _Core._6_Characters.Enemies;
using _Framework.SOEventSystem.Events;
using DG.Tweening;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerCombat : Destructible
    {
        #region vars

        #region PlayerCombat Vars

        [SerializeField] private bool debugMode;
        
        [Header("Combat")] 
        [SerializeField] private GameObject attackArea;
        [SerializeField] private Vector2 knockbackStrength;
        public float LastPressedAttackTime { get; set; }
        private float lastAttackedTime;

        #endregion

        #region components
        [Header("Components")]
        [SerializeField] private VoidEvent playerTookDamageEvent;

        private PlayerAnimator animator;
        private PlayerCombatData playerData;
        private PlayerMovement movement;
        private Rigidbody2D rb;

        #endregion

        #endregion

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<PlayerAnimator>();
            movement = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody2D>();
            playerData = (PlayerCombatData) data;

            if (debugMode) playerData.currentHealth = playerData.maxHealth;
            
            health = playerData.currentHealth;
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

        public override void OnAttackHit(int damage)
        {
            // damage handling
            TakeKnockback();
            
            // reduce hp
            base.OnAttackHit(damage);

            if (playerData.currentHealth != health)
            {
                playerData.currentHealth = health;
                
                // invoke event
                playerTookDamageEvent.Invoke();
            }
        }
        
        private void TakeKnockback()
        {
            movement.IgnoreRun = true;
            var direction = movement.IsFacingRight ? -1 : 1;
            var force = new Vector2(knockbackStrength.x * direction, knockbackStrength.y);
            
            rb.velocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);

            DOVirtual.DelayedCall(0.5f, () => movement.IgnoreRun = false);
        }
    }
}