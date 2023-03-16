using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class DashAttack : EnemyAction
    {
        [SerializeField] private float preparationDuration;
        [SerializeField] private float buildUpTime;
        [SerializeField] private float dashDuration;
    

        private Tween startPreparation;
        private Tween startBuildUp;
        private Tween startAttack;
        private Tween finishAttack;

        private bool playerToRight;
        private bool dashAttackFinished;

        public override void OnStart()
        {
            playerToRight = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer());
            var position = playerToRight ? -20 : 20;
        
            var finalPrepareDuration = Vector2.Distance(transform.position, new Vector2(position, 0)) / 20 * preparationDuration;
        
            bossEnemy.CheckDirectionToFace(!playerToRight);
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);
       
            startPreparation = transform.DOMoveX(position, finalPrepareDuration)
                .SetEase(Ease.Linear)
                .OnComplete(StartBuildup);
        }

        private void StartBuildup()
        {
            bossEnemy.CheckDirectionToFace(playerToRight);
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossDashBuildup);
            startBuildUp = DOVirtual.DelayedCall(buildUpTime, startDashing);

        }

        private void startDashing()
        {
            var position = playerToRight ? 20 : -20;
        
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossSwingAttack);
        
            startAttack = transform.DOMoveX(position, dashDuration)
                .SetEase(Ease.Linear)
                .OnComplete(FinishAttack);
        }

        private void FinishAttack()
        {
            finishAttack = DOVirtual.DelayedCall(bossEnemy.ChangeAnimationState(BossAnimatorState.BossSwingAttackEnd), () => dashAttackFinished = true);
        }
    
        public override TaskStatus OnUpdate()
        {
            return dashAttackFinished ? TaskStatus.Success : TaskStatus.Running;    
        }
        public override void OnEnd()
        {
            dashAttackFinished = false;
            startPreparation.Kill();
            startBuildUp.Kill();
            startAttack.Kill();
            finishAttack.Kill();
        }
    }
}
