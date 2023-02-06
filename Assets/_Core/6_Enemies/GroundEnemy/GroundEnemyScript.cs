using _Core._6_Enemies.ScriptableObjects;
using UnityEngine;

namespace _Core._6_Enemies.GroundEnemy
{
    public class GroundEnemyScript : MonoBehaviour, IEnemy
    {
        #region vars

        #region Movement vars

        [Header("Movement")] 
        [SerializeField] private float aggroRange;
        [SerializeField] private float accelRate;
        [SerializeField] private float idleSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallVelocity;
        [SerializeField] private float normalGravity;

        private float distanceToPlayer;
        public bool isFacingRight { get; private set; }

        #endregion

        #region Check vars

        [Header("Checks")] 
        private bool isWalled;
        private bool isGrounded;
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private LayerMask wallAndGroundCheckLayer;

        private const float WallCheckRadius = 0.2f;
        private const float GroundCheckRadius = 0.2f;

        #endregion

        #region Component vars

        [Header("Components")] 
        [SerializeField] private EnemyData enemyData;

        private Rigidbody2D rb;
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
            EnemyAI();
            Jump();
        }

        #endregion

        #region Chasing

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
        }

        private void ChasePlayer()
        {
            var direction = transform.position.x < player.transform.position.x ? Vector2.right : Vector2.left;

            rb.AddForce(accelRate * direction, ForceMode2D.Force);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, enemyData.enemySpeed);
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
            var targetSpeed = Mathf.Sign(difference) * idleSpeed;
            rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
        }

        #endregion

        #region Checks

        private bool CollisionCheck(Transform checkPoint, float radius)
        {
            return Physics2D.OverlapCircle(checkPoint.position, radius, wallAndGroundCheckLayer);
        }

        private void GetDistToPlayer()
        {
            if (!ReferenceEquals(player, null))
            {
                distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            }
        }

        private void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != isFacingRight)
            {
                Flip();
            }
        }

        #endregion

        #region misc

        private void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }

        private void Jump()
        {
            if (isWalled && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (rb.velocity.y < 0 && !isGrounded)
            {
                rb.gravityScale = fallVelocity;
            }
            else
            {
                rb.gravityScale = normalGravity;
            }
        }

        #endregion
    }
}