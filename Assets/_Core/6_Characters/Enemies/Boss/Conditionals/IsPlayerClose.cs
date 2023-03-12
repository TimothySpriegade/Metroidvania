using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Conditionals
{
    public class IsPlayerClose : EnemyConditional
    {
        [SerializeField] private float maxDistance;

        public override TaskStatus OnUpdate()
        {
            var distanceToPlayer = TargetUtils.GetDistToTarget(gameObject, boss.GetPlayer());

            return distanceToPlayer < maxDistance ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
