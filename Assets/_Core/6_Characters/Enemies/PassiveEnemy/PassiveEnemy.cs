using _Core._5_Player;
using UnityEngine;

namespace _Core._6_Characters.Enemies.PassiveEnemy
{
   public class PassiveEnemy : AbstractEnemy
    {
        #region vars

        #region Movement vars
        [Header("Movement")]

        [SerializeField] private float accelRate;

        private float moveSpeed;
        private bool isOnTopOfEnemy;


        #endregion

        #region Idle Vars
        [Header("Idle")]

        [SerializeField] private Transform[] idlePoints;

        private int index;

        #endregion

        #region Check Vars
        [Header("Cheks")]

        [SerializeField] private Transform groundCheckPoint;

        private const float GroundCheckRadius = 0.2f;

        #endregion

        #endregion

        #region UnityMethods



        private void Update()
        {
            if (rb.velocity.x != 0 && !isOnTopOfEnemy)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }


            if (!duringAnimation && !isOnTopOfEnemy)
            {
                EnemyAI();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

            CapSpeed(moveSpeed);

        }

        #endregion

        #region EnemyAI

        protected override void EnemyAI()
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

  
        #endregion

        #region Animation
        public override float DeathAnimation()
        {
            return 0;
        }
        #endregion

        #region checks

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(groundCheckPoint.position, GroundCheckRadius);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            isOnTopOfEnemy = col.GetContact(0).normal.y < -0.5f;
            
            if (!isOnTopOfEnemy && col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerCombat>().OnAttackHit(enemyData.damage);
            }
        }
        private void OnCollisionExit2D(Collision2D col)
        {
            isOnTopOfEnemy = false;
        }


    }
    public enum PassiveEnemyAnimatorState
    {
        PassiveEnemyRun,
        PassiveEnemyDeath
    }
}