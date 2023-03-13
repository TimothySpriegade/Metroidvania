using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss
{
    public class BossEnemy : AbstractEnemy
    {
        private BossAnimatorState currentState;
        protected override void EnemyAI()
        {
            // Behavior Tree
        }

        public override float DeathAnimation()
        {
            return 0;
        }

        public float ChangeAnimationState(BossAnimatorState newState)
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
    }

    public enum BossAnimatorState
    {
        BossIdle,
        BossExhausted,
        BossDeath,
        BossRun,
        BossSmallAttack,
        BossLargeAttack,
        BossSwingAttack,
        BossSwingAttackEnd,
        BossJumpAttack,
        BossWarning,
        BossDashBuildup
    }
}
