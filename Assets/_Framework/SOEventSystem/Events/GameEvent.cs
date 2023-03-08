using System;
using UnityEngine;

namespace _Framework.SOEventSystem.Events
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        [field: ContextMenuItem("Raise", "Raise")]

        public event Action<T> EventListeners = delegate {};

        public void Invoke(T item)
        {
            EventListeners(item);
        }
    }
}