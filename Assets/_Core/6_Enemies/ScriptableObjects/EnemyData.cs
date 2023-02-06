using UnityEngine;

namespace _Core._6_Enemies.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Enemy/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float enemyMaxHealth;
        public float enemySpeed;
        public float enemyDamage;
    }
}