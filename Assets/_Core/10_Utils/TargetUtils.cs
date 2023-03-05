using Unity.VisualScripting;
using UnityEngine;

namespace _Core._10_Utils
{
    public static class TargetUtils
    {
        public static float GetDistToTarget(Vector2 position, GameObject target)
        {
            if (!TargetExists(target)) return 0;
            
            return Vector2.Distance(position, target.transform.position);
        }

        public static bool TargetIsToRight(Vector2 position, GameObject target)
        {
            if (!TargetExists(target)) return false;
                
            return position.x < target.transform.position.x;
        }

        public static bool TargetExists(Object target)
        {
            return !ReferenceEquals(target, null) && !target.IsDestroyed();
        }
    }
}