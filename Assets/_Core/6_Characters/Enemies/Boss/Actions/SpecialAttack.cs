using System;
using System.Collections.Generic;
using _Core._5_Player;
using _Core._6_Characters.Enemies.Boss.AI;
using _Framework;
using _Framework.SOEventSystem;
using _Framework.SOEventSystem.Events;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class SpecialAttack : EnemyAction
    {
        [Header("Special Attack Start")]
        [SerializeField] private float startDuration;
        
        [SerializeField] private CameraShakeEvent cameraShakeEvent;
        [SerializeField] private Vector2 setNewPlayerCheckpoint;

        [Space(5)]
        [Header("Special Attack Preparation")] 
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject spikeGround;
        private List<GameObject> spawnedPlatforms;

        [Space(5)] 
        [Header("Special Attack")] 
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int projectileCount;
        [SerializeField] private float projectileDelay;
        [SerializeField] private float projectileTimeToLive;

        private Collider2D[] projectileSpawners;
        private bool finishedAttack;
        private float gravityScale;
        private Collider2D collider;
        private Tween attackTween;
        private Tween moveTween;
        private Tween endTween;

        public override void OnStart()
        {
            // Getting components
            collider = GetComponent<Collider2D>();
            projectileSpawners = GetComponent<BoneSpawner>().GetBoneSpawners();
            
            if (projectileSpawners.Length == 0)
            {
                throw new ArgumentException("Missing projectile spawners");
            }
            
            // Stuff like, no gravity, invincible etc
            collider.enabled = false;
            gravityScale = rb.gravityScale;
            rb.gravityScale = 0;
            bossCombat.Invincible = true;
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossIdle);

            // Camera Shake
            var cameraShakeConfig = new CameraShakeConfiguration(2, 2, startDuration);
            cameraShakeEvent?.Invoke(cameraShakeConfig);
            bossCombat.GetPlayer().GetComponent<PlayerCheckpointController>().SetNewPosition(setNewPlayerCheckpoint);

            // Move upwards and then start preparation
            moveTween = transform.DOMoveY(8, startDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(StartPreparation);
        }

        private void StartPreparation()
        {
            // Spawning platforms
            var verticalPosition = 2;
            spawnedPlatforms = new List<GameObject>();
            
            for (var i = 0; i < 3; i++)
            {
                var prefab = Object.Instantiate(platformPrefab);
                prefab.transform.position = new Vector2(0, verticalPosition);
                spawnedPlatforms.Add(prefab);
                verticalPosition += 5;
            }
            
            // Activating Spike Ground
            spikeGround.SetActive(true);
        }
        
        private void StartAttack()
        {
            attackTween = DOTween.Sequence()
                .AppendCallback(SpawnProjectile)
                .AppendInterval(projectileDelay)
                .SetLoops(projectileCount)
                .Play();
            
            gameObject.Log($"Finished creating projectiles. Ending attack in {projectileTimeToLive} seconds");
            endTween = DOVirtual.DelayedCall(projectileTimeToLive, () => finishedAttack = true);
        }

        private void SpawnProjectile()
        {
            var spawnerIndex = Random.Range(0, projectileSpawners.Length);
            var chosenSpawner = projectileSpawners[spawnerIndex];
            
            gameObject.Log("Spawn! chosenSpawner height is: " + chosenSpawner.transform.position.y);
        }
        

        public override TaskStatus OnUpdate()
        {
            return finishedAttack ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            collider.enabled = true;
            rb.gravityScale = gravityScale;
            bossCombat.Invincible = false;
            spikeGround.SetActive(false);
            attackTween?.Kill();
            moveTween?.Kill();
            endTween?.Kill();
        }
    }
}