using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class GoToMiddle : EnemyAction
    {
        [SerializeField] private float walkDuration;
        private Tween walkTween;
        private bool finishedWalking;
        
        public override void OnStart()
        {
            var finalWalkDuration =  (Mathf.Abs(transform.position.x) / 20) * walkDuration;
            
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);
            walkTween = transform.DOMoveX(0, finalWalkDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => finishedWalking = true);
        }

        public override TaskStatus OnUpdate()
        {
            return finishedWalking ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            walkTween?.Kill();
        }
    }
}