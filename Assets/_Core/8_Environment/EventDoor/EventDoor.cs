using DG.Tweening;
using UnityEngine;

namespace _Core._8_Environment.EventDoor
{
    public class EventDoor : MonoBehaviour
    {
        public void OpenEventDoor()
        {
            transform.DOMoveY(-4, 3).SetRelative();
        }
    }
}