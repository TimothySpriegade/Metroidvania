using UnityEngine;

namespace _Framework
{
    public static class Logger
    {
        public static void Log(this Object loggingObject, object message)
        {
#if UNITY_EDITOR
            var className = loggingObject.name;
            message = $"{className} {message}";
            Debug.Log(message);
#endif
        }
    }
}