using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class ChangeAnimation : EnemyAction
    {
        [SerializeField] private BossAnimatorState bossAnimatorState;

        public override void OnStart()
        {
            bossEnemy.ChangeAnimationState(bossAnimatorState);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
