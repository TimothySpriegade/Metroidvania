using SOEventSystem.Events;
using UnityEngine;

namespace SOEventSystem.Tester
{
    public class EventTester : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private string stringInput = "";
        [SerializeField] private float floatInput = 0;

        [Header("Events")]
        [SerializeField] private VoidEvent voidEvent; 
        [SerializeField] private FloatEvent floatEvent; 
        [SerializeField] private StringEvent stringEvent;

        public void Invoke()
        {
            if (voidEvent != null)
            {
                voidEvent.Invoke();
            }
            if (floatEvent != null)
            {
                floatEvent.Invoke(floatInput);
            } 
            else if (stringEvent != null)
            {
                stringEvent.Invoke(stringInput);
            }
            
        }
    }
}