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
        [Header("Checks")]

        [SerializeField] private Transform groundCheckPoint;

        private PassiveEnemyAnimatorState currentState;

        private const float GroundCheckRadius = 0.2f;

        #endregion

        #endregion

        #region UnityMethods



        private void Update()
        {
            if (rb.velocity.x != 0 && !duringAnimation && !isOnTopOfEnemy)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }


            if (duringAnimation) return;
            
            if (!isOnTopOfEnemy)
            {
                ChangeAnimationState(PassiveEnemyAnimatorState.PassiveEnemyRun);
                EnemyAI();
            }
            else
            {
                ChangeAnimationState(PassiveEnemyAnimatorState.PassiveEnemySteppedOn);
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
            moveSpeed = combat.enemyData.idleSpeed;
        }

  
        #endregion

        #region Animation
        private float ChangeAnimationState(PassiveEnemyAnimatorState newState)
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
            duringAnimation = true;
            combat.Invincible = true;
            return ChangeAnimationState(PassiveEnemyAnimatorState.PassiveEnemyDeath);
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
                col.gameObject.GetComponent<PlayerCombat>().OnAttackHit(combat.enemyData.damage, gameObject);
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
        PassiveEnemyDeath,
        PassiveEnemySteppedOn
    }
}