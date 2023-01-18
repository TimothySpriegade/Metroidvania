using System;
using SOEventSystem.Events;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Menus.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        #region vars
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private StringEvent onBackToMainMenu;
        #endregion

        public void OpenPauseMenu()
        {
            Time.timeScale = 0;
            menu.SetActive(true);
        }

        public void ContinueGame()
        {
            menu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void GoOptionsMenu()
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }

        public void BackToMainMenu()
        {
            onBackToMainMenu?.Invoke("MainMenu");
        }
    
       
    }
}
