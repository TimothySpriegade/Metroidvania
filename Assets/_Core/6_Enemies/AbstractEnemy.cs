using System;
using _Core._6_Enemies.ScriptableObjects;
using UnityEngine;

namespace _Core._6_Enemies
{
    public abstract class AbstractEnemy : MonoBehaviour, IEnemy
    {
        #region Variables

        public bool isFacingRight { get; private set; }
        public bool duringAnimation { get; set; }
        
        protected Rigidbody2D rb;

        [SerializeField] protected EnemyData enemyData;
        [SerializeField] protected LayerMask collisionCheckLayer;

        #endregion

        private void Awake()
        {
            try
            {
                rb.GetComponent<Rigidbody2D>();
            }
            catch (Exception e)
            {
                Debug.LogError("All Enemies need to have a Rigidbody2D");
                throw;
            }
            
            OnStarting();
        }

        protected virtual void OnStarting()
        {
        }

        #region Flip Logic

        protected void CheckDirectionToFace(bool isMovingRight)
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
            var newVerticalVelocity = Mathf.Clamp(rb.velocity.y, -enemyData.maxFallSpeed, maxJump);
            rb.velocity = new Vector2(newHorizontalVelocity, newVerticalVelocity);
        }

        protected void CapSpeed(float maxSpeed)
        {
            var newVelocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            rb.velocity = newVelocity;
        }

        protected abstract void EnemyAI();

        #endregion

    }
}
