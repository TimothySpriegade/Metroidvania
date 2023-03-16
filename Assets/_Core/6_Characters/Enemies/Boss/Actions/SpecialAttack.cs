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
        [Header("Special Attack Start")] [SerializeField]
        private float startDuration;

        [SerializeField] private CameraShakeEvent cameraShakeEvent;
        [SerializeField] private Vector2 setNewPlayerCheckpoint;

        [Space(5)] [Header("Special Attack Preparation")] 
        [SerializeField] private float preparationDuration;

        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject spikeGround;
        private List<GameObject> spawnedPlatforms;

        [Space(5)] [Header("Special Attack")] 
        [SerializeField] private GameObject projectilePrefab;

        [SerializeField] private int projectileCount;
        [SerializeField] private float projectileDelay;

        [Header("End Attack")] 
        [SerializeField] private float endAttackDelay;
        

        private Collider2D[] projectileSpawners;
        private float gravityScale;
        private Collider2D collider;
        private bool finishedAttack;
        private Collider2D lastCollider;

        private Tween attackTween;
        private Tween moveTween;
        private Tween preparationTween;
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
            bossEnemy.ChangeAnimationState(BossAnimatorState.BossSpecialAttack);

            // Camera Shake
            var cameraShakeConfig = new CameraShakeConfiguration(2, 2, startDuration);
            cameraShakeEvent?.Invoke(cameraShakeConfig);
            
            // Setting Player Checkpoint to lowest platform
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

            preparationTween = DOVirtual.DelayedCall(preparationDuration, StartAttack);
        }

        private void StartAttack()
        {
            attackTween = DOTween.Sequence()
                .AppendCallback(SpawnProjectiles)
                .AppendInterval(projectileDelay)
                .SetLoops(projectileCount)
                .OnComplete(EndAttackDelay);
        }

        private void SpawnProjectiles()
        {
            var chosenSpawner = PickSpawner();
            
            var bounds = chosenSpawner.bounds;
            var spawnPositionX = chosenSpawner.transform.position.x;
            var spawnPositionY = Random.Range(bounds.min.y, bounds.max.y);

            var projectile = Object.Instantiate(projectilePrefab);
            projectile.transform.position = new Vector2(spawnPositionX, spawnPositionY);
            var projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.InitializeMovement(true);
            projectileScript.SetDamage(0);
        }

        private Collider2D PickSpawner()
        {
            var chosenSpawner = lastCollider;
            
            while (chosenSpawner == lastCollider)
            {
                var spawnerIndex = Random.Range(0, projectileSpawners.Length);
                chosenSpawner = projectileSpawners[spawnerIndex];
            }

            lastCollider = chosenSpawner;
            return chosenSpawner;
        }

        private void EndAttackDelay()
        {
            // Deactivating Spike Ground
            spikeGround.SetActive(false);
            
            foreach (var spawnedPlatform in spawnedPlatforms)
            {
                Object.Destroy(spawnedPlatform);
            }
            
            gameObject.Log($"Finished creating projectiles. Ending attack in {endAttackDelay} seconds");
            endTween = DOVirtual.DelayedCall(endAttackDelay, () => finishedAttack = true);
        }


        public override TaskStatus OnUpdate()
        {
           
            return finishedAttack ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            // Setting Attack
            bossEnemy.lastAttack = BossAttack.SpecialAttack;
            // Resetting values
            finishedAttack = false;
            collider.enabled = true;
            rb.gravityScale = gravityScale;
            bossCombat.Invincible = false;
            spikeGround.SetActive(false);
            attackTween?.Kill();
            moveTween?.Kill();
            preparationTween?.Kill();
            endTween?.Kill();
        }
    }
}