using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using _Framework.SOEventSystem;
using _Framework.SOEventSystem.Events;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class JumpAttack : EnemyAction
    {
        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private CameraShakeEvent cameraShake;
        
        [SerializeField] private float buildupTimeMin;
        [SerializeField] private float buildupTimeMax;
        
        [SerializeField] private float jumpTime;
        [SerializeField] private float endTime;

        private bool hasLanded;
        private Tween jumpTween;
        private Tween landingTween;
        private Tween landingLagTween;

        public override void OnStart()
        {
            var randomBuildupTime = Random.Range(buildupTimeMin, buildupTimeMax);
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossWarning);
            jumpTween = DOVirtual.DelayedCall(randomBuildupTime, StartJump, false);
        }

        private void StartJump()
        {
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossJumpAttack);
            
            var direction = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer()) ? 1 : -1;
            var force = new Vector2(jumpForce.x * direction, jumpForce.y);
            rb.AddForce(force, ForceMode2D.Impulse);

            landingTween = DOVirtual.DelayedCall(jumpTime, () =>
            {
                EndJump();
                cameraShake.Invoke(new CameraShakeConfiguration(8, 3));
            }, false);
        }

        private void EndJump()
        {
            landingLagTween = DOVirtual.DelayedCall(endTime, () => hasLanded = true);
        }

        public override TaskStatus OnUpdate()
        {
            return hasLanded ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            hasLanded = false;
            jumpTween?.Kill();
            landingTween?.Kill();
        }
    }
}