using _Framework;
using SOEventSystem.Events;
using UnityEngine;

namespace _Core._4_Menus.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        #region vars

        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private StringEvent onSceneChange;

        #endregion

        public void OpenPauseMenu()
        {
            this.Log("Opening Pause Menu");
            Time.timeScale = 0;
            menu.SetActive(true);
        }

        public void ContinueGame()
        {
            this.Log("Closing Pause Menu");
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
            onSceneChange.Invoke("MainMenu");
        }
    }
}