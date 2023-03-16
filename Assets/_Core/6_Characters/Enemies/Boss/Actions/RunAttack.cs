using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class RunAttack : EnemyAction
    {
        [SerializeField] private float wantedDistanceToPlayer;
        [SerializeField] private float walkDuration;
        [SerializeField] private float reactionTime;

        private Tween walkTween;
        private Tween prepareAttackTween;
        private Tween finishAttackAnimationTween;

        private bool finishedAttack;


        public override void OnStart()
        {
            finishedAttack = false;
            var distanceToPlayer = TargetUtils.GetDistToTarget(gameObject, bossCombat.GetPlayer());
            var wantedPosition = distanceToPlayer - wantedDistanceToPlayer;
            wantedPosition = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer()) ? wantedPosition : -wantedPosition;
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);
            walkTween = transform.DOMoveX(wantedPosition, walkDuration)
                .SetEase(Ease.Linear)
                .SetRelative()
                .OnComplete(PrepareAttack);
        }

        private void PrepareAttack()
        {
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossIdle);
            prepareAttackTween = DOVirtual.DelayedCall(reactionTime, Attack);
        }

        private void Attack()
        {
            finishAttackAnimationTween = DOVirtual.DelayedCall(bossEnemy.ChangeAnimationState(BossAnimatorState.BossSmallAttack), () => finishedAttack = true);
        }

        public override TaskStatus OnUpdate()
        {
            return finishedAttack ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            bossEnemy.lastAttack = BossAttack.RunAttack;
            
            walkTween?.Kill();
            prepareAttackTween?.Kill();
            finishAttackAnimationTween?.Kill();
        }


    }
}
