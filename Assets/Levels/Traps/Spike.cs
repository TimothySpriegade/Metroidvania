using SOEventSystem.Events;
using UnityEngine;

namespace Levels.Traps
{
    public class Spike : MonoBehaviour
    {
        [SerializeField] private FloatEvent onSpikeHit;
        [SerializeField] private VoidEvent onSpikeLeave;
        [SerializeField] private SpikeData data;


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                onSpikeHit.Invoke(data.damage);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                onSpikeLeave.Invoke();
            }
        }
    }
}
