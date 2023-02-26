using _Core._6_Characters.Enemies.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
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
            if (rb.velocity.x != 0)
            {
                CheckDirectionToFace(rb.velocity.x > 0);
            }


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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(groundCheckPoint.position, GroundCheckRadius);
        }

    }
    public enum PassiveEnemyAnimatorState
    {
        PassiveEnemyRun,
        PassiveEnemyDeath
    }
}