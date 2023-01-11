using SOEventSystem.Events;
using UnityEngine;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private StringEvent onSceneChange;
        public void PlayGame()
        {
            //lädt nächste Szene in Scenemanager - änder sich vllt noch wegen save
            onSceneChange.Invoke("TestLevel");
        }
        public static void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
            Application.Quit();
        }
    }
}
