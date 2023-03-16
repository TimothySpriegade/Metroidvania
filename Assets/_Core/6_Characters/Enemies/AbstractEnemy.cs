using _Core._5_Player.ScriptableObjects;
using UnityEngine;

namespace _Core._6_Characters.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Destructible))]
    public abstract class AbstractEnemy : MonoBehaviour, IEnemy
    {
        #region Variables

        public bool isFacingRight { get; private set; }
        public bool duringAnimation { get; set; }


        [SerializeField] protected PlayerReferenceData playerData;
        [SerializeField] protected LayerMask collisionCheckLayer;
        protected Rigidbody2D rb;
        protected EnemyCombat combat;
        protected Animator animator;

        #endregion

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            combat = GetComponent<EnemyCombat>();
            animator = GetComponentInChildren<Animator>();
        }

        #region Flip Logic

        public void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != isFacingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }

        #endregion

        #region Misc

        protected bool CollisionCheck(Transform checkPoint, float radius)
        {
            return Physics2D.OverlapCircle(checkPoint.position, radius, collisionCheckLayer);
        }

        protected void CapSpeed(float maxSpeed, float maxJump)
        {
            var newHorizontalVelocity = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
            var newVerticalVelocity = Mathf.Clamp(rb.velocity.y, -combat.enemyData.maxFallSpeed, maxJump);
            rb.velocity = new Vector2(newHorizontalVelocity, newVerticalVelocity);
        }

        protected void CapSpeed(float maxSpeed)
        {
            var newVelocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            rb.velocity = newVelocity;
        }

        protected abstract void EnemyAI();
        public abstract float DeathAnimation();

        #endregion
    }
}