using EventSystem.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private StringEvent onSceneChange;
        public void PlayGame()
        {
            //lädt nächste Szene in Scenemanager - änder sich vllt noch wegen save
            onSceneChange.Invoke("TestLevel");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public static void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
