using UnityEngine;
using UnityEngine.Events;

namespace EventSystem
{
    //E = Event / UER = Unity Event Response
    public abstract class GameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> 
        where E : GameEvent<T> 
        where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent;
        public E GameEvent { get => gameEvent; set => gameEvent = value; }
        
        [SerializeField] private UER unityEventResponse;


        private void OnEnable()
        { if(GameEvent != null) GameEvent.RegisterListener(this); }

        private void OnDisable()
        { if(GameEvent != null) GameEvent.UnregisterListener(this); }

        public void OnEventRaised(T item)
        {
            unityEventResponse.Invoke(item);
        }
    }
}