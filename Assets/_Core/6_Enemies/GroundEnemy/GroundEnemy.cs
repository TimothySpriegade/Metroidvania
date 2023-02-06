using Unity.VisualScripting;
using UnityEngine;

namespace _Core._6_Enemies.GroundEnemy
{
    public class GroundEnemy : AbstractFlippableEnemy
    {
        #region vars

        #region Movement vars

        [Header("Movement")] 
        [SerializeField] private float aggroRange;
        [SerializeField] private float accelRate;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallVelocity;
        [SerializeField] private float normalGravity;

        private float distanceToPlayer;

        #endregion

        #region Check vars

        [Header("Checks")] 
        private bool isWalled;
        private bool isGrounded;
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private Transform groundCheckPoint;
        

        private const float WallCheckRadius = 0.2f;
        private const float GroundCheckRadius = 0.2f;

        #endregion

        #region Component vars

        private GameObject player;

        #endregion

        #region Idle vars

        [Header("Idle")] 
        [SerializeField] private Transform[] idlePoints;
        private int index;

        #endregion

        #endregion

        #region UnityMethods

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (rb.velocity.x != 0)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }

            isWalled = CollisionCheck(wallCheckPoint, WallCheckRadius);
            isGrounded = CollisionCheck(groundCheckPoint, GroundCheckRadius);
            GetDistToPlayer();
            
            if (!duringAnimation)
            {
                EnemyAI();
                Jump();
            }

            rb.gravityScale = IsFalling() ? fallVelocity : normalGravity;
        }

        #endregion

        #region Enemy AI

        private void EnemyAI()
        {
            if (distanceToPlayer <= aggroRange)
            {
                ChasePlayer();
            }
            else
            {
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
            CapSpeed(enemyData.chaseSpeed, jumpForce);
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
            CapSpeed(enemyData.idleSpeed, jumpForce);
        }

        private void GetDistToPlayer()
        {
            if (!ReferenceEquals(player, null) && !player.IsDestroyed())
            {
                distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            }
        }

        private void Jump()
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        #endregion

        #region Checks

        private bool IsFalling()
        {
            return rb.velocity.y < 0 && !isGrounded;
        }

        #endregion
        
    }
}