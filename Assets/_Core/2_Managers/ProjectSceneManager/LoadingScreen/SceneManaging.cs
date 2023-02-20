using _Framework;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Core._2_Managers.ProjectSceneManager.LoadingScreen
{
    public class SceneManaging : MonoBehaviour
    {
        private Animator loadingAnimator;
        private static readonly int OnSceneChange = Animator.StringToHash("onSceneChange");

        private void Start()
        {
            this.Log($"{SceneManager.GetActiveScene().name} loaded.");
            loadingAnimator = GetComponent<Animator>();
        }

        public void LoadNextLevel(string scene)
        {
            this.Log($"Loading scene {scene}");
            loadingAnimator.SetTrigger(OnSceneChange);
            DOVirtual.DelayedCall(loadingAnimator.GetCurrentAnimatorStateInfo(0).length,
                () => SceneManager.LoadScene(scene));
        }
    }
}