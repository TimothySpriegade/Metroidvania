using _Core._6_Enemies.ScriptableObjects;
using UnityEngine;

namespace _Core._6_Enemies.GroundEnemy
{
    public class GroundEnemyScript : MonoBehaviour
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
        [SerializeField] private float maxSpeed;
        private float distanceToPlayer;

        #endregion

        #region Check vars

        [Header("Checks")] 
        [SerializeField] private bool isWalled;
        [SerializeField] private bool isGrounded;
        [SerializeField] private Transform wallCheckCollider;
        [SerializeField] private Transform groundCheckCollider;
        [SerializeField] private LayerMask wallAndGroundCheckLayer;
        private const float WallCheckRadius = 0.2f;

        private const float GroundCheckRadius = 0.2f;

        // TODO value assigned but never used
        private bool isFacingRight;
        private GameObject player;

        #endregion

        #region Component vars

        [Header("Components")] private Rigidbody2D rb;
        [SerializeField] private EnemyData enemyData;

        #endregion

        #region idle vars

        [Header("Idle")] 
        [SerializeField] private Transform[] idlePoints;
        private int index;

        #endregion

        #endregion

        #region UnityMethods

        // Start is called before the first frame update
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        private void Update()
        {
            if (rb.velocity.x != 0)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }

            WallCheck();
            GroundCheck();
            GetDistToPlayer();
            EnemyAI();
            Jump();
        }

        #endregion

        #region Chasing

        private void EnemyAI()
        {
            if (distanceToPlayer < aggroRange)
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
            if (transform.position.x < player.transform.position.x)
            {
                rb.AddForce(accelRate * Vector2.right, ForceMode2D.Force);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, enemyData.enemySpeed);
            }
            else
            {
                rb.AddForce(accelRate * Vector2.left, ForceMode2D.Force);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, enemyData.enemySpeed);
            }
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

        private void WallCheck()
        {
            var colliders =
                Physics2D.OverlapCircleAll(wallCheckCollider.position, WallCheckRadius, wallAndGroundCheckLayer);
            if (colliders.Length > 0)
            {
                isWalled = true;
            }
            else
            {
                isWalled = false;
            }

            // TODO alternative:
            //isWalled = Physics2D.OverlapCircle(wallCheckCollider.position, WallCheckRadius, wallAndGroundCheckLayer);
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

        private void GroundCheck()
        {
            isGrounded =
                Physics2D.OverlapCircle(groundCheckCollider.position, GroundCheckRadius, wallAndGroundCheckLayer);
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
            if (isWalled)
            {
                rb.velocity = Vector2.up * jumpForce;
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