using _Core._2_Managers.GameManager.PlayerSpawner;
using DG.Tweening;
using UnityEngine;

namespace _Core._11_Credits
{
    public class CreditsLevelTransition : LevelTransition
    {
        private Animator animator;
        private Tween delayedSceneTransitionTween;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                animator.SetTrigger($"OnStart");
            }

            delayedSceneTransitionTween = DOVirtual.DelayedCall(2, () => base.OnTriggerEnter2D(col));
        }

        private void OnDisable()
        {
            delayedSceneTransitionTween?.Kill();
        }
    }
}
