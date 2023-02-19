using UnityEngine;

namespace _Core._6_Enemies.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Enemy/EnemyData")]
    public class EnemyData : DestructibleData
    {
        public float chaseSpeed;
        public float idleSpeed;
        public float maxFallSpeed;
        public float damage;
    }
}