using UnityEngine;

namespace _Core._2_Managers.GameManager.PlayerSpawner.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Level Transition/Level Transition Data")]
    public class LevelTransitionData : ScriptableObject
    {
        public LevelData lastLevel;
        public bool playerWasFacingRight;
    }
}