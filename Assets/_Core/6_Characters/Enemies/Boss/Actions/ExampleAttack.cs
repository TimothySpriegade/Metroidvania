﻿using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using _Framework.SOEventSystem;
using _Framework.SOEventSystem.Events;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class ExampleAttack : EnemyAction
    {
        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private CameraShakeEvent cameraShake;
        
        [SerializeField] private float buildupTime;
        [SerializeField] private float jumpTime;

        private bool hasLanded;
        private Tween jumpTween;
        private Tween landingTween;

        public override void OnStart()
        {
            jumpTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
        }

        private void StartJump()
        {
            var direction = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer()) ? 1 : -1;
            var force = new Vector2(jumpForce.x * direction, jumpForce.y);
            rb.AddForce(force, ForceMode2D.Impulse);

            landingTween = DOVirtual.DelayedCall(jumpTime, () =>
            {
                hasLanded = true;
                cameraShake.Invoke(new CameraShakeConfiguration(8, 3));
            }, false);
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