using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class GoTowardsMiddle : EnemyAction
    {
        [SerializeField] private float walkDistance;
        [SerializeField] private float walkDuration;
        private Tween walkTween;
        private bool finishedWalking;
        
        public override void OnStart()
        {
            var direction = Mathf.Sign(transform.position.x) == 1 ? -1 : 1;
            
            
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);


            walkTween = transform.DOMoveX(walkDistance * direction, walkDuration)
                .SetEase(Ease.Linear)
                .SetRelative()
                .OnComplete(() => finishedWalking = true);


        }

        public override TaskStatus OnUpdate()
        {
            return finishedWalking ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            walkTween?.Kill();
            finishedWalking = false;
        }
    }
}
