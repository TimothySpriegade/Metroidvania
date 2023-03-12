using Unity.VisualScripting;
using UnityEngine;

namespace _Core._10_Utils
{
    public static class TargetUtils
    {
        public static float GetDistToTarget(GameObject source, GameObject target)
        {
            if (!TargetExists(target)) return 0;
            
            return Vector2.Distance(source.transform.position, target.transform.position);
        }

        public static bool TargetIsToRight(GameObject source, GameObject target)
        {
            if (!TargetExists(target)) return false;
                
            return source.transform.position.x < target.transform.position.x;
        }

        public static bool TargetExists(GameObject target)
        {
            return !ReferenceEquals(target, null) && !target.IsDestroyed();
        }
    }
}