using _Core._10_Utils;
using _Core._5_Player;
using UnityEngine;

namespace _Core._6_Characters.Enemies.GroundEnemy
{
    public class GroundEnemy : AbstractEnemy
    {
        #region vars

        #region Movement vars

        [Header("Movement")] 
        [SerializeField] private float aggroRange;
        [SerializeField] private float accelRate;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallVelocity;
        [SerializeField] private float normalGravity;
        private float moveSpeed;
        private float debugAccelRate;
        #endregion

        #region Check vars

        [Header("Checks")] 
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private Transform groundCheckPoint;
        
        private bool isWalled;
        private bool isGrounded;
        
        private const float WallCheckRadius = 0.2f;
        private const float GroundCheckRadius = 0.2f;

        #endregion
        

        #region Animation

        private GroundEnemyAnimatorState currentState;

        #endregion

        #region Idle vars

        [Header("Idle")]
        [SerializeField] private Transform[] idlePoints;
        private int index;
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

            if (isIdle && Mathf.Abs(transform.position.x - idlePoints[index].position.x) < 0.1f)
            {
                index++;
                if (index == idlePoints.Length)
                {
                    index = 0;
                }
            }
            
            isWalled = CollisionCheck(wallCheckPoint, WallCheckRadius);
            isGrounded = CollisionCheck(groundCheckPoint, GroundCheckRadius);

            rb.gravityScale = IsFalling() ? fallVelocity : normalGravity;
        }

        private void FixedUpdate()
        {
            if (!duringAnimation)
            {
                EnemyAI();
                CapSpeed(moveSpeed, jumpForce);
            }
        }

        #endregion

        #region Enemy AI

        protected override void EnemyAI()
        {
            if (TargetUtils.TargetExists(playerData.player) && TargetUtils.GetDistToTarget(gameObject, playerData.player) <= aggroRange)
            {
                ChangeAnimationState(GroundEnemyAnimatorState.GroundEnemyChase);
                ChasePlayer();
                isIdle = false;
            }
            else
            {
                ChangeAnimationState(GroundEnemyAnimatorState.GroundEnemyRun);
                Idle();
                isIdle = true;
            }

            if (isWalled && isGrounded)
            {
                Jump();
            }
        }

        private void ChasePlayer()
        {
            var direction = TargetUtils.TargetIsToRight(gameObject, playerData.player) ? Vector2.right : Vector2.left;

            rb.AddForce(accelRate * Time.fixedDeltaTime * 200 * direction, ForceMode2D.Force);
            moveSpeed = combat.enemyData.chaseSpeed;
        }

        private void Idle()
        {
            var difference = idlePoints[index].position.x - transform.position.x;
            var targetSpeed = Mathf.Sign(difference) * accelRate * Time.fixedDeltaTime * 200;
            rb.AddForce(Vector2.right * targetSpeed, ForceMode2D.Force);
            moveSpeed = combat.enemyData.idleSpeed;
        }

        

        private void Jump()
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        #endregion

        #region Animation

        private float ChangeAnimationState(GroundEnemyAnimatorState newState)
        {
            //Stop if currently played Animation matches attempted animation
            if (currentState == newState) return 0;

            //Play animation
            animator.Play(newState.ToString());

            //replace currentState
            currentState = newState;
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override float DeathAnimation()
        {   
            duringAnimation = true;
            combat.Invincible = true;
            return ChangeAnimationState(GroundEnemyAnimatorState.GroundEnemyDeath);
        }

        #endregion

        #region Checks

        private bool IsFalling()
        {
            return rb.velocity.y < 0 && !isGrounded;
        }

        #endregion

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerCombat>().OnAttackHit(combat.enemyData.damage, gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(wallCheckPoint.position, WallCheckRadius);
            Gizmos.DrawWireSphere(groundCheckPoint.position, GroundCheckRadius);
        }
    }

    public enum GroundEnemyAnimatorState
    {
        GroundEnemyRun,
        GroundEnemyChase,
        GroundEnemyDeath
    }
}