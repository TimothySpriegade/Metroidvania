using UnityEngine;

namespace _Core._5_Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Player/PlayerCombatData")]
    public class PlayerCombatData : ScriptableObject
    {
        public float maxHealth;
        public float damage;
    }
}
