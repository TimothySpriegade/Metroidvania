using SOEventSystem.Events;
using UnityEngine;

namespace _Core._4_Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private StringEvent onSceneChange;

        public void PlayGame(string levelName)
        {
            //lädt nächste Szene in Scenemanager - änder sich vllt noch wegen save
            onSceneChange.Invoke(levelName);
        }

        public static void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}