using Unity.VisualScripting;
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

        #region Component vars

        private GameObject player;
        private Animator animator;

        #endregion

        #region Animation

        private GroundEnemyAnimatorState currentState;

        #endregion

        #region Idle vars

        [Header("Idle")]
        [SerializeField] private Transform[] idlePoints;
        private int index;

        #endregion

        #endregion

        #region UnityMethods

        protected override void Start()
        {
            base.Start();
            player = GameObject.FindGameObjectWithTag("Player");
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (rb.velocity.x != 0)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }

            isWalled = CollisionCheck(wallCheckPoint, WallCheckRadius);
            isGrounded = CollisionCheck(groundCheckPoint, GroundCheckRadius);

            if (!duringAnimation)
            {
                EnemyAI();
            }

            CapSpeed(moveSpeed, jumpForce);

            rb.gravityScale = IsFalling() ? fallVelocity : normalGravity;
        }

        #endregion

        #region Enemy AI

        protected override void EnemyAI()
        {
            if (GetDistToPlayer() <= aggroRange)
            {
                ChangeAnimationState(GroundEnemyAnimatorState.GroundEnemyChase);
                ChasePlayer();
            }
            else
            {
                ChangeAnimationState(GroundEnemyAnimatorState.GroundEnemyRun);
                Idle();
            }

            if (isWalled && isGrounded)
            {
                Jump();
            }
        }

        private void ChasePlayer()
        {
            var direction = transform.position.x < player.transform.position.x ? Vector2.right : Vector2.left;

            rb.AddForce(accelRate * direction, ForceMode2D.Force);
            moveSpeed = enemyData.chaseSpeed;
        }

        private void Idle()
        {
            if (Mathf.Abs(transform.position.x - idlePoints[index].position.x) < 0.02f)
            {
                index++;
                if (index == idlePoints.Length)
                {
                    index = 0;
                }
            }

            var difference = idlePoints[index].position.x - transform.position.x;
            var targetSpeed = Mathf.Sign(difference) * accelRate;
            rb.AddForce(Vector2.right * targetSpeed, ForceMode2D.Force);
            moveSpeed = enemyData.idleSpeed;
        }

        private float GetDistToPlayer()
        {
            if (!ReferenceEquals(player, null) && !player.IsDestroyed())
            {
                return Vector2.Distance(transform.position, player.transform.position);
            }

            return aggroRange + 1;
        }

        private void Jump()
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        #endregion

        #region Animation

        private void ChangeAnimationState(GroundEnemyAnimatorState newState)
        {
            //Stop if currently played Animation matches attempted animation
            if (currentState == newState) return;

            //Play animation
            animator.Play(newState.ToString());

            //replace currentState
            currentState = newState;
        }

        public override float DeathAnimation()
        {
            ChangeAnimationState(GroundEnemyAnimatorState.GroundEnemyDeath);
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }

        #endregion

        #region Checks

        private bool IsFalling()
        {
            return rb.velocity.y < 0 && !isGrounded;
        }

        #endregion

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