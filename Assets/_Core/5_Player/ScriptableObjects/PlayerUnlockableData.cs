using UnityEngine;

namespace _Core._5_Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Player/PlayerUnlockableData")]
    public class PlayerUnlockableData : ScriptableObject
    {
        public bool unlockedDash;

        private void OnEnable()
        {
            unlockedDash = false;
        }
    }
}