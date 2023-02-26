using UnityEngine;

namespace _Core._5_Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Player/PlayerCombatData")]
    public class PlayerCombatData : ScriptableObject
    {
        public int maxHealth;
        public int damage;
        public int currentHealth;
    }
}
