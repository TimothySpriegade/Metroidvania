using _Core._6_Characters.Enemies.ScriptableObjects;
using UnityEngine;

namespace _Core._5_Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Player/PlayerCombatData")]
    public class PlayerCombatData : DestructibleData
    {
        public int currentHealth;
        public int damage;

        private void OnEnable()
        {
            currentHealth = maxHealth;
        }
    }
}
