using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class LookToMiddle : EnemyAction
    {
        public override void OnStart()
        {
            var targetIsToRight = transform.position.x < 0;
            
            bossEnemy.CheckDirectionToFace(targetIsToRight);
        }
        
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}