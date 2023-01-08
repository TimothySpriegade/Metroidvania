using UnityEngine;

namespace Player.ScriptableObjects
{
    public class PlayerControlData : ScriptableObject
    {
        [HideInInspector] public PlayerControls Controls;
        private void OnValidate()
        {
            Controls ??= new PlayerControls();
        }

        public void ChangeControls()
        {
        
            GameObject.Find("Player").GetComponent<PlayerController>().UpdateControls();
        }
    }
}