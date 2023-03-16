using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Conditionals
{
    public class NoDuplicateAttack : EnemyConditional
    {
        [SerializeField] private BossAttack wantedAttack;
        
        public override TaskStatus OnUpdate()
        {
            return wantedAttack != bossEnemy.lastAttack ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}