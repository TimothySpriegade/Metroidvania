using EventSystem.Events;
using UnityEngine;
using UnityEngine.Events;

namespace EventSystem.Listeners
{
    //E = Event / UER = Unity Event Response
    public abstract class GameEventListener<T, E, UER> : MonoBehaviour
        where E : GameEvent<T> 
        where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent;
        
        [SerializeField] private UER unityEventResponse;


        private void OnEnable()
        {
            if (gameEvent != null) gameEvent.EventListeners += OnEventRaised;
        }

        private void OnDisable()
        {
            if (gameEvent != null) gameEvent.EventListeners -= OnEventRaised;
        }

        public void OnEventRaised(T item)
        {
            unityEventResponse.Invoke(item);
        }
    }
}