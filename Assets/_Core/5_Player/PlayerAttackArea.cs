using _Core._5_Player.ScriptableObjects;
using _Core._6_Characters.Enemies;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerAttackArea : MonoBehaviour
    {
        [SerializeField] private PlayerCombatData data;
        
        private const string Destructible = "Destructible";
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag(Destructible))
            {
                var destructible = (Destructible)col.gameObject.GetComponent(typeof(Destructible));
                destructible.OnDamageTaken(data.damage);
            }
        }
    }
}
