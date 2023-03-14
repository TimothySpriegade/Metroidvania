using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class GoToMiddle : EnemyAction
    {
        [SerializeField] private float walkDuration = 1;
        private Tween walkTween;
        
        public override void OnStart()
        {
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossRun);
            walkTween = transform.DOMoveX(0, walkDuration).SetEase(Ease.Linear);
        }

        public override TaskStatus OnUpdate()
        {
            return walkTween.active ? TaskStatus.Running : TaskStatus.Success;
        }

        public override void OnEnd()
        {
            walkTween?.Kill();
        }
    }
}