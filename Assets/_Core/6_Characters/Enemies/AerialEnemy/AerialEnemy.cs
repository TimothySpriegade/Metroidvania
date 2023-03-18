using _Core._10_Utils;
using _Core._5_Player;
using UnityEngine;

namespace _Core._6_Characters.Enemies.AerialEnemy {
    public class AerialEnemy : AbstractEnemy
    {
        #region vars

        #region Movement Vars
        [Header("Movement")]

        [SerializeField] private float aggroRange;
        [SerializeField] private float accelRate;
        private float moveSpeed;

        #endregion

        #region Idle Vars
        [Header("Idle")]

        [SerializeField] private Transform[] idlePoints;
        private int index;
        private FlyingEnemyAnimatorState currentState;
        private bool isIdle;

        #endregion

        #endregion

        #region UnityMethods
        
        private void Update()
        {
            if (rb.velocity.x != 0)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }
            
            if (isIdle && Vector2.Distance(transform.position, idlePoints[index].position) < 0.1f)
            {
                index++;
                if (index == idlePoints.Length)
                {
                    index = 0;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!duringAnimation)
            {
                EnemyAI();
                CapSpeed(moveSpeed);
            }
        }

        #endregion

        #region EnemyAI
        protected override void EnemyAI()
        {
            if (TargetUtils.TargetExists(playerData.player) && TargetUtils.GetDistToTarget(gameObject, playerData.player) <= aggroRange)
            {
                ChangeAnimationState(FlyingEnemyAnimatorState.FlyingEnemyChase);
                ChasePlayer();
                isIdle = false;
            }
            else
            {
                ChangeAnimationState(FlyingEnemyAnimatorState.FlyingEnemyIdle);
                Idle();
                isIdle = true;
            }
        }

        private void ChasePlayer()
        {
            var difference = playerData.player.transform.position - transform.position;
            var targetSpeed = difference.normalized * (accelRate * Time.fixedDeltaTime * 200);
            rb.AddForce(targetSpeed, ForceMode2D.Force);
            
            moveSpeed = combat.enemyData.chaseSpeed;
        }

        private void Idle()
        {
            var difference = idlePoints[index].position - transform.position;
            var targetSpeed = difference.normalized * (accelRate * Time.fixedDeltaTime * 200);
            rb.AddForce(targetSpeed, ForceMode2D.Force);
            moveSpeed = combat.enemyData.idleSpeed;
        }

        #endregion

        #region Animation
        
        private float ChangeAnimationState(FlyingEnemyAnimatorState newState)
        {
            //Stop if currently played Animation matches attempted animation
            if (currentState == newState) return 0;

            //Play animation
            animator.Play(newState.ToString());
            animator.Update(Time.smoothDeltaTime);
        
            //replace currentState
            currentState = newState;
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }
        
        public override float DeathAnimation()
        {
            GetComponent<Collider2D>().enabled = false;
            duringAnimation = true;
            combat.Invincible = true;
            rb.drag = 5;
            return ChangeAnimationState(FlyingEnemyAnimatorState.FlyingEnemyDeath);
        }
        
        #endregion

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerCombat>().OnAttackHit(combat.enemyData.damage, gameObject);
            }
        }
    }
    public enum FlyingEnemyAnimatorState
    {
        FlyingEnemyIdle,
        FlyingEnemyChase,
        FlyingEnemyDeath
    }
}
