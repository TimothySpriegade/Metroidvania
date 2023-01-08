using EventSystem;
using UnityEngine;


namespace Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/PlayerControlData")]
    public class PlayerControlData : ScriptableObject
    {
        [HideInInspector] public PlayerControls Controls;
        
        private void OnValidate()
        {
            Controls ??= new PlayerControls();
        }
    }
}