using SOEventSystem.Events;
using UnityEngine;

namespace Levels.Traps
{
    public class EnvironmentalTrap : MonoBehaviour
    {
        [SerializeField] private FloatEvent onEnvironmentalTrapHit;
        [SerializeField] private EnvironmentalTrapData data;


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                onEnvironmentalTrapHit.Invoke(data.damage);
            }
        }
    }
}
