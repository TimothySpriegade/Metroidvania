using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;

namespace _Core._6_Characters.Enemies.Boss.Conditionals
{
    public class BossIsDead : EnemyConditional
    {
        public override TaskStatus OnUpdate()
        {
            return boss.health <= 0 ? TaskStatus.Running : TaskStatus.Failure;
        }
    }
}
