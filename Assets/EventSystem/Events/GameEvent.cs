using System;
using UnityEngine;

namespace EventSystem.Events
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        public event Action<T> EventListeners = delegate {};

        public void Raise(T item)
        {
            EventListeners(item);
        }
    }
}