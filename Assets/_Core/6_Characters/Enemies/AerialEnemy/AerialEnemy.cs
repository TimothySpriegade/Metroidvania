using _Core._5_Player;
using Unity.VisualScripting;
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

        #endregion

        #region Check Vars

        [Header("Checks")]
        [SerializeField] private Transform collidingPoint;
        [SerializeField] private float collidingRadius = 5;

        private bool isColliding;

        #endregion

        #region Component Vars

        private GameObject player;

        #endregion

        #endregion

        #region UnityMethods

        protected override void Start()
        {
            base.Start();
            player = GameObject.FindGameObjectWithTag("Player");
        }
        private void Update()
        {
            if (rb.velocity.x != 0)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }

            isColliding = CollisionCheck(collidingPoint, collidingRadius);

            if (!duringAnimation)
            {
                EnemyAI();
            }
            CapSpeed(moveSpeed);
        }
        #endregion

        #region EnemyAI
        protected override void EnemyAI()
        {
            if (GetDistToPlayer() <= aggroRange)
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
            var difference = player.transform.position - transform.position;
            var targetSpeed = difference.normalized * accelRate;
            rb.AddForce(targetSpeed, ForceMode2D.Force);
            
            moveSpeed = enemyData.chaseSpeed;
        }

        private void Idle()
        {
            if (Vector2.Distance(transform.position, idlePoints[index].position) < 0.02f)
            {
                index++;
                if (index == idlePoints.Length)
                {
                    index = 0;
                }
            }

            var difference = idlePoints[index].position - transform.position;
            var targetSpeed = difference.normalized * accelRate;
            rb.AddForce(targetSpeed, ForceMode2D.Force);
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
      

        #endregion

        #region Animation
        public override float DeathAnimation()
        {
            return 0;
        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(collidingPoint.position, collidingRadius);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerCombat>().OnAttackHit(enemyData.damage);
            }
        }
    }
    public enum AerialEnemyAnimatorState
    {
        AerialEnemyIdle,
        AerialEnemyChase,
        AerialEnemyDeath
    }
}
