using _Core._5_Player.ScriptableObjects;
using _Framework;
using UnityEngine;
using UnityEngine.UI;

namespace _Core._9_UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        #region vars
        #region Health Vars

        [Header("Health")] 
        [SerializeField] private Image[] hearts;
        [SerializeField] private Sprite fullHearts;
        [SerializeField] private Sprite emptyHearts;
        
        #endregion

        #region Data

        [Header("Data")] 
        [SerializeField] private PlayerCombatData playerData;

        #endregion

        #endregion

        #region healthMethods

        private void Start()
        {
            HealthSpriteUpdater();
        }

        public void HealthSpriteUpdater()
        {
            for (var i = 0; i < hearts.Length; i++)
            {
                playerData.currentHealth = playerData.currentHealth > playerData.maxHealth
                    ? playerData.maxHealth
                    : playerData.currentHealth;

                hearts[i].sprite = i < playerData.currentHealth ? fullHearts : emptyHearts;

                hearts[i].enabled = i < playerData.maxHealth;
            }
            
            this.Log("Updated PlayerHealth");
        }
        
        #endregion
    }
}