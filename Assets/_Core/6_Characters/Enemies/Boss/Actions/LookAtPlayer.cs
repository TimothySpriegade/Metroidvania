using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class LookAtPlayer : EnemyAction
    {
        public override void OnStart()
        {
            var targetIsToRight = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer());
            
            bossEnemy.CheckDirectionToFace(targetIsToRight);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}