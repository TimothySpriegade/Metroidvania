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
            onSceneChange.Invoke("TestLevel07");
        }
        public static void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
