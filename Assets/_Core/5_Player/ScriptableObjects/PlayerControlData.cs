using UnityEngine;

namespace _Core._5_Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Player/PlayerControlData")]
    public class PlayerControlData : ScriptableObject
    {
        private PlayerControls controls;

        [HideInInspector]
        public PlayerControls Controls
        {
            get
            {
                controls ??= new PlayerControls();
                return controls;
            }
            set => controls = value;
        }
    }
}