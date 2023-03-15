using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class ExhaustionState : EnemyAction
    {
        [SerializeField] private float exhaustionLength = 10;
        private float exhaustionTimer;

        public override void OnStart()
        {
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossExhausted);
        }
        public override TaskStatus OnUpdate()
        {
            exhaustionTimer += Time.deltaTime;
        
            if(exhaustionTimer >= exhaustionLength) 
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
