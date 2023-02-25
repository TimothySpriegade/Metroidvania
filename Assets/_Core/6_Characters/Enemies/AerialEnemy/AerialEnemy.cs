using _Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Core._6_Characters.Enemies.AerialEnemy {
    public class AerialEnemy : AbstractEnemy
    {
        //TODO: find combat issue (no knockback or destroy), fix backtracking

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

        protected override void OnStarting()
        {

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
            var directionx = transform.position.x < player.transform.position.x ? Vector2.right : Vector2.left;
            var directiony = transform.position.y < player.transform.position.y ? Vector2.up : Vector2.down;

            rb.AddForce(accelRate * directionx, ForceMode2D.Force);
            rb.AddForce(accelRate * directiony, ForceMode2D.Force);
            moveSpeed = enemyData.chaseSpeed;
        }

        private void Idle()
        {
            if (Mathf.Abs(transform.position.y - idlePoints[index].position.y) < 0.02f)
            {
                index++;
                if (index == idlePoints.Length)
                {
                    index = 0;
                }
            }

            var differencex = idlePoints[index].position.x - transform.position.x;
            var differencey = idlePoints[index].position.y - transform.position.y;
            var targetSpeedx = Mathf.Sign(differencex) * accelRate;
            var targetSpeedy = Mathf.Sign(differencey) * accelRate;

            rb.AddForce(Vector2.right * targetSpeedx, ForceMode2D.Force);
            rb.AddForce(Vector2.up * targetSpeedy, ForceMode2D.Force);

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

        #region Checks

        #endregion

        #region Animation
        public override float DeathAnimation()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(collidingPoint.position, collidingRadius);
        }

    }
    public enum AerialEnemyAnimatorState
    {
        AerialEnemyFly,
        AerialEnemyDeath
    }
}
