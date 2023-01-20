using UnityEngine;

namespace Spawners.PlayerSpawner.ScriptableObject
{
    [CreateAssetMenu(menuName = "Data/Level Transition/Level Transition Data")]
    public class LevelTransitionData : UnityEngine.ScriptableObject
    {
        public LevelData lastLevel;
        public TransitionDirection direction;
    }
}
