using System.Collections;
using Spawners.PlayerSpawner;
using UnityEngine;

namespace Player
{
    //Doing animations and animated methods (I.e. cutscenes etc.)
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator animator;
        private PlayerMovement movement;
        private PlayerController controller;
        private Rigidbody2D rb;

        private void Start()
        {
            //animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }


        #region Event Handling

        public void OnEnvironmentalTrapHit()
        {
            StartCoroutine(controller.ShortControlSuspension());
        }

        #endregion

        #region New scene

        public void EnteringSceneAnimation(TransitionDirection transitionDirection)
        {
            StartCoroutine(transitionDirection == TransitionDirection.Up
                ? UpTransitionAnimation()
                : SideTransitionAnimation());
        }

        private IEnumerator UpTransitionAnimation()
        {
            controller.DisableAllControls();
            yield return null;
        }
        
        private IEnumerator SideTransitionAnimation()
        {
            controller.DisableAllControls();
            yield return null;
        }

        #endregion
    }
}
