using System.Collections.Generic;
using _Core._5_Player;
using _Core._6_Characters.Enemies.Boss.AI;
using _Framework.SOEventSystem;
using _Framework.SOEventSystem.Events;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class SpecialAttack : EnemyAction
    {
        [Header("Special Attack Start")]
        [SerializeField] private float startDuration;
        
        [SerializeField] private CameraShakeEvent cameraShakeEvent;
        [SerializeField] private Vector2 setNewPlayerCheckpoint;

        [Header("Special Attack Preparation")] 
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject spikeGround;
        private List<GameObject> spawnedPlatforms;


        [Header("Special Attack")]
        [SerializeField] private int projectileCount;
        [SerializeField] private float projectileDelay;

        private Collider2D[] projectileSpawners;
        private bool finishedAttack;
        private float gravityScale;
        private Collider2D collider;
        private Tween attackTween;
        private Tween moveTween;

        public override void OnStart()
        {
            // Getting components
            collider = GetComponent<Collider2D>();
            projectileSpawners = GetComponent<BoneSpawner>().GetBoneSpawners();
            
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
            var attackTween = DOTween.Sequence()
                .AppendCallback(SpawnProjectile)
                .AppendInterval(projectileDelay)
                .SetLoops(projectileCount)
                .Play();
        }

        private void SpawnProjectile()
        {
            var spawnerIndex = Random.Range(0, projectileSpawners.Length);
            var chosenSpawner = projectileSpawners[spawnerIndex];
            
            Debug.Log("Spawn!");
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
            attackTween?.Kill();
            moveTween?.Kill();
        }
    }
}