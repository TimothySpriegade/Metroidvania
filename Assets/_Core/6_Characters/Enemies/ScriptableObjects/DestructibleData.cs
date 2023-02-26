using UnityEngine;

namespace _Core._6_Characters.Enemies.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Enemy/DestructibleData")]
    public class DestructibleData : ScriptableObject
    {
        public int maxHealth;
    }
}