using _Framework.SOEventSystem.Events;
using UnityEngine;

namespace _Core._8_Environment.EventDoor
{
    public class EventTrigger : MonoBehaviour
    {
        [SerializeField] private VoidEvent onTriggerEnter;

        private void OnTriggerEnter2D(Collider2D col)
        {
            onTriggerEnter?.Invoke();
            Destroy(gameObject);
        }
    }
}
