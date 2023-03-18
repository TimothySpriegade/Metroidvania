using _Core._5_Player.ScriptableObjects;
using _Framework.SOEventSystem.Events;
using UnityEngine;

namespace _Core._10_Utils
{
    public class CheatMode : MonoBehaviour
    {
        [SerializeField] private PlayerCombatData data;
        [SerializeField] private VoidEvent updateUI;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                data.currentHealth = data.maxHealth;
                updateUI.Invoke();
            }
        }
    }
}
