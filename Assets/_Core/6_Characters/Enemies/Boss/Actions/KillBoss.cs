using _Core._6_Characters.Enemies.Boss.AI;
using _Framework.SOEventSystem;
using _Framework.SOEventSystem.Events;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class KillBoss : EnemyAction
    {
        [SerializeField] private VoidEvent onBossDeath;
        [SerializeField] private CameraShakeEvent cameraShake;
        private bool finishedDying;

        public override void OnStart()
        {
            GetComponent<Collider2D>().enabled = false;
            rb.gravityScale = 0;
            var duration = bossEnemy.ChangeAnimationState(BossAnimatorState.BossDeath);
            var cameraShakeConfiguration = new CameraShakeConfiguration(2f, 2f, duration + 1);
            
            cameraShake.Invoke(cameraShakeConfiguration);
            DOVirtual.DelayedCall(duration, () =>
            {
                onBossDeath.Invoke();
                finishedDying = true;
            });
        }

        public override TaskStatus OnUpdate()
        {
            return finishedDying ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}
