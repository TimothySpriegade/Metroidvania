using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class ChangeAnimation : EnemyAction
    {
        [SerializeField] private BossAnimatorState bossAnimatorState;
        private bool changedAnimation;

        public override void OnStart()
        {
            bossEnemy.ChangeAnimationState(bossAnimatorState);

            changedAnimation = true;
        }

        public override TaskStatus OnUpdate()
        {
            return changedAnimation ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}
