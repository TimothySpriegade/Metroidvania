using DG.Tweening;
using UnityEngine;

namespace _Core._8_Environment.EventDoor
{
    public class EventDoor : MonoBehaviour
    {
        [SerializeField] private float amount;
        [SerializeField] private float duration;
        public void MoveEventDoor()
        {
            transform.DOMoveY(amount, duration).SetRelative();
        }
    }
}