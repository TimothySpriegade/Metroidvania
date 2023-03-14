using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class LookToMiddle : EnemyAction
    {
        private bool hasTurned;

        public override void OnStart()
        {
            var targetIsToRight = transform.position.x < 0;
            
            bossEnemy.CheckDirectionToFace(targetIsToRight);

            hasTurned = true;
        }
        
        public override TaskStatus OnUpdate()
        {
            return hasTurned ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}