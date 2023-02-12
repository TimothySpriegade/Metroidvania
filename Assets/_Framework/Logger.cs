using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Framework
{
    public static class Logger
    {
        public static void Log(this Object loggingObject, object message)
        {
#if UNITY_EDITOR
            message = CombinedMessage(loggingObject, message);
            Debug.Log(message);
#endif
        }

        public static void LogWarning(this Object loggingObject, object message)
        {
#if UNITY_EDITOR
            message = CombinedMessage(loggingObject, message);
            Debug.LogWarning(message);
#endif
        }
        
        public static void LogError(this Object loggingObject, object message, Exception e)
        {
#if UNITY_EDITOR
            message = CombinedMessage(loggingObject, message);
            Debug.LogError(message);
            Debug.LogException(e);
#endif
        }

        private static object CombinedMessage(Object loggingObject, object message)
        {
            var className = loggingObject.name;
            return $"[{className}] {message}";
        }
    }
}