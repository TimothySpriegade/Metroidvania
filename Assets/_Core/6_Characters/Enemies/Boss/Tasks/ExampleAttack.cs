using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Tasks
{
    public class ExampleAttack : EnemyAction
    {
        [SerializeField] private Vector2 jumpForce;

        [SerializeField] private float buildupTime;
        [SerializeField] private float jumpTime;

        public override void OnStart()
        {
            DOVirtual.DelayedCall(buildupTime, StartJump, false);
            destructible = (BossCombat) GetComponent<>()
        }

        private void StartJump()
        {
            var direction = 
        }

        public override TaskStatus OnUpdate()
        {
            
        }
    }
}
