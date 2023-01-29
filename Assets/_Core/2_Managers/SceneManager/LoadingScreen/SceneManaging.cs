using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.LoadingScreen
{
    public class SceneManaging : MonoBehaviour
    {
        private Animator loadingAnimator;

        private void Start()
        {
            loadingAnimator = GetComponent<Animator>();
        }

        public void LoadNextLevel(string scene)
        {
            StartCoroutine(LoadLevel(scene));
        }

        private IEnumerator LoadLevel(string scene)
        {
            loadingAnimator.SetTrigger("onSceneChange");
            yield return new WaitForSeconds(loadingAnimator.GetCurrentAnimatorStateInfo(0).length);
            
            SceneManager.LoadScene(scene);
        }
    }
}
