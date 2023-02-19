using SOEventSystem.Events;
using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem.Listeners
{
    //E = GameEvent / UER = Unity Event Response
    public abstract class GameEventListener<T, E, UER> : MonoBehaviour
        where E : GameEvent<T> 
        where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent;
        
        [SerializeField] private UER unityEventResponse;


        private void OnEnable()
        {
            if (gameEvent != null) gameEvent.EventListeners += OnEventInvoked;
        }

        private void OnDisable()
        {
            if (gameEvent != null) gameEvent.EventListeners -= OnEventInvoked;
        }

        private void OnEventInvoked(T item)
        {
            unityEventResponse.Invoke(item);
        }
    }
}