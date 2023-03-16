using _Core._10_Utils;
using _Core._6_Characters.Enemies.Boss.AI;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.Actions
{
    public class SpawnFallingProjectiles : EnemyAction
    {
        [SerializeField] private Collider2D spawnBox;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int projectileCount;
        [SerializeField] private float projectileDelay;

        private Tween spawnTween;
        private bool playerToRight;
        private bool finishedSpawning;
        
        
        public override void OnStart()
        {
            playerToRight = TargetUtils.TargetIsToRight(gameObject, bossCombat.GetPlayer());

            spawnTween = DOTween.Sequence()
                .AppendCallback(SpawnProjectiles)
                .AppendInterval(projectileDelay)
                .SetLoops(projectileCount)
                .OnComplete(() => finishedSpawning = true);
        }

        private void SpawnProjectiles()
        {
            // Determine Spawn Position
            var horizontalSpawnLocation = playerToRight
                ? Random.Range(transform.position.x, spawnBox.bounds.max.x)
                : Random.Range(spawnBox.bounds.min.x, transform.position.x);

            var spawnLocation = new Vector2(horizontalSpawnLocation, spawnBox.transform.position.y);
            
            
            // Create Projectile
            var projectile = Object.Instantiate(projectilePrefab);
            projectile.transform.position = spawnLocation;

            var projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.InitializeMovement(false);
        }

        public override TaskStatus OnUpdate()
        {
            return finishedSpawning ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            spawnTween?.Kill();
            finishedSpawning = false;
        }
    }
}