using _Core._5_Player;
using _Framework.SOEventSystem.Events;
using UnityEngine;

namespace _Core._8_Environment.Traps
{
    public class EnvironmentalTrap : MonoBehaviour
    {
        [SerializeField] private VoidEvent onEnvironmentalTrapHit;
        [SerializeField] private EnvironmentalTrapData data;


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerCombat>().OnEnvironmentalTrapHitCallback(data.damage);
            }
        }
    }
}