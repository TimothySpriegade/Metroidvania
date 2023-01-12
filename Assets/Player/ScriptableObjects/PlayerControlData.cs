using UnityEngine;


namespace Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/PlayerControlData")]
    public class PlayerControlData : ScriptableObject
    {
        [HideInInspector] public PlayerControls Controls { get; set; }
        
        private void Awake()
        {
            Controls ??= new PlayerControls();
        }
    }
}