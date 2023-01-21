using System.Collections;
using Spawners.PlayerSpawner;
using UnityEngine;

namespace Player
{
    //Doing animations and animated methods (I.e. cutscenes etc.)
    public class PlayerAnimator : MonoBehaviour
    {

        #region Components

        private Animator animator;
        private PlayerMovement movement;
        private PlayerController controller;
        private Rigidbody2D rb;

        #endregion

        #region Animation Vars

        [SerializeField] private Vector2 upAnimationForce;
        private bool collisionDetected;

        #endregion
        
        
        private void Awake()
        {
            //animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<PlayerMovement>();
            controller = GetComponent<PlayerController>();
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            collisionDetected = true;
        }


        #region Event Handling

        public void OnEnvironmentalTrapHit()
        {
            StartCoroutine(controller.ShortControlSuspension());
        }

        #endregion

        #region New scene transitions

        

        public void EnteringSceneAnimation(TransitionDirection transitionDirection)
        {
            if (transitionDirection == TransitionDirection.Up)
            {
                StartCoroutine(UpTransitionAnimation());
            } 
            else
            {
                var direction = transitionDirection == TransitionDirection.Right ? 1 : -1;
                StartCoroutine(SideTransitionAnimation(direction));
            }
        }

        private IEnumerator UpTransitionAnimation()
        {
            //Disables Controls
            movement.IgnoreRun = true;
            controller.DisableAllControls();
            
            //Adds Force
            rb.AddForce(upAnimationForce, ForceMode2D.Impulse);
            
            //Keeps Controls Enabled until the player hits something
            while (!collisionDetected)
            {
                yield return null;
            }

            //Activates Controls
            collisionDetected = false;
            movement.IgnoreRun = false;
            controller.EnableAllControls();
        }
        
        private IEnumerator SideTransitionAnimation(int direction)
        {
            //Disables Controls
            movement.IgnoreRun = true;
            controller.DisableAllControls();

            var startTime = Time.time;
            
            //Sets player velocity sidewards for .6 seconds
            while (Time.time - startTime <= 0.6f)
            {
                rb.velocity = new Vector2(5 * direction, rb.velocity.y);
                yield return null;
            }
            
            //Activates Controls
            movement.IgnoreRun = false;
            controller.EnableAllControls();
        }

        #endregion
    }
}
