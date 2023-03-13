using _Core._5_Player;
using UnityEngine;

namespace _Core._8_Environment.Traps
{
    [RequireComponent(typeof(Collider2D))]
    public class Hazard : MonoBehaviour
    {
        [SerializeField] private int damage;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerCombat>().OnAttackHit(damage, gameObject);
            }
        }
    }
}