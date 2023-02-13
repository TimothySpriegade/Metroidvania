using _Framework;
using SOEventSystem.Events;
using UnityEngine;

namespace _Core._4_Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private StringEvent onSceneChange;

        public void PlayGame(string levelName)
        {
            //starting the game by loading scene with the given levelName
            this.Log("Starting game");
            onSceneChange.Invoke(levelName);
        }

        public void QuitGame()
        {
            this.Log("Quitting game");
            Application.Quit();
        }
    }
}