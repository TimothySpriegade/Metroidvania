using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;

namespace _Core._6_Characters.Enemies.Boss.Tasks
{
    public class LookAtPlayer : EnemyAction
    {
        private bool hasTurned;

        public override void OnStart()
        {
            var targetIsToRight = TargetUtils.TargetIsToRight(gameObject, boss.GetPlayer());
            
            boss.BossDto.CheckDirectionToFace(targetIsToRight);

            hasTurned = true;
        }
        
        public override TaskStatus OnUpdate()
        {
            return hasTurned ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}