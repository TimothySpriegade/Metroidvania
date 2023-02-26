using _Framework.SOEventSystem.Events;
using UnityEngine;

namespace _Framework.SOEventSystem.Tester
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
            // ReSharper disable Unity.NoNullPropagation
            voidEvent?.Invoke();
            floatEvent?.Invoke(floatInput);
            stringEvent?.Invoke(stringInput);
            
        }
    }
}