using _Core._10_Utils;
using _Core._5_Player.ScriptableObjects;
using _Core._6_Characters.Enemies;
using _Framework.SOEventSystem;
using _Framework.SOEventSystem.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Core._5_Player
{
    public class PlayerCombat : Destructible
    {
        #region vars

        #region PlayerCombat Vars


        [Header("Combat")]
        [SerializeField] private Vector2 knockbackStrength;

        [SerializeField] private int attackAmountUntilHeal;
        public float LastPressedAttackTime { get; set; }
        public bool isAttacking { get; private set; }
        private float lastAttackedTime;
        private static int attackCount;

        #endregion

        #region components

        [Header("Components")] 
        [SerializeField] private StringEvent onSceneChange;
        [FormerlySerializedAs("playerTookDamageEvent")] [SerializeField] private VoidEvent playerHealthChanged;
        [SerializeField] private CameraShakeEvent cameraShakeEvent;
        private PlayerCombatData playerData;
        private PlayerAnimator animator;
        private PlayerMovement movement;
        private Rigidbody2D rb;
        private Tween deathTween;

        #endregion

        #endregion

        #region UnityMethods

        protected override void Awake()
        {
            playerData = (PlayerCombatData)data;
#if UNITY_EDITOR

            playerData.currentHealth = playerData.maxHealth;
#endif 
            
            base.Awake();
            animator = GetComponent<PlayerAnimator>();
            movement = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody2D>();
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
            return lastAttackedTime <= 0 && !movement.isDashing;
        }

        private void Attack()
        {
            // Preparation
            LastPressedAttackTime = 0;

            // Animation
            var attackLength = animator.ChangeAnimationState(PlayerAnimatorState.PlayerAttack);

            // Activating attack hitbox
            isAttacking = true;
            lastAttackedTime = attackLength;

            // Deactivating attack hitbox
            DOVirtual.DelayedCall(attackLength, () => isAttacking = false);
        }

        public override void OnAttackHit(int damage, GameObject attacker)
        {
            if (Invincible) return;
            
            // damage feedback handling
            TakeKnockback(attacker);

            // reduce hp
            TakeDamage(damage, attacker);

            // invoke event
            playerHealthChanged.Invoke();
        }

        private void TakeKnockback(GameObject attacker)
        {
            movement.IgnoreRun = true;

            var direction = TargetUtils.TargetIsToRight(gameObject, attacker) ? -1 : 1;
             
            var force = new Vector2(knockbackStrength.x * direction, knockbackStrength.y);

            rb.velocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);

            DOVirtual.DelayedCall(0.5f, () => movement.IgnoreRun = false);
        }

        protected override void Destroy()
        {
            if (deathTween is {active: true}) return;
            
            deathTween = DOVirtual.DelayedCall(animator.ChangeAnimationState(PlayerAnimatorState.PlayerDeath), () =>
            {
                playerData.currentHealth = playerData.maxHealth;
                onSceneChange.Invoke("MainMenu");
            });
        }

        public void OnAttackHitEventCallback()
        {
            attackCount++;

            if (attackCount >= attackAmountUntilHeal)
            {
                Heal(1);
                playerData.currentHealth = health;
                attackCount = 0;
                playerHealthChanged.Invoke();
            }
        }

        public void OnEnvironmentalTrapHitCallback(int damage)
        {
            TakeDamage(damage, gameObject);
            
            // invoke event
            playerHealthChanged.Invoke();
        }

        private void TakeDamage(int damage, GameObject attacker)
        {
            base.OnAttackHit(damage, attacker);
            playerData.currentHealth = health;
            
            // Camera shake
            var cameraShakeConfig = new CameraShakeConfiguration(5, 3, 0.5f);
            cameraShakeEvent.Invoke(cameraShakeConfig);
        }

        private void OnDisable()
        {
            deathTween?.Kill();
        }
    }
}