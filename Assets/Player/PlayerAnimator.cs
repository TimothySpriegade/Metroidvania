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

        private PlayerAnimatorState currentState;

        #endregion

        private Vector2 velocity; //TODO remove debug
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<PlayerMovement>();
            controller = GetComponent<PlayerController>();
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            collisionDetected = true;
        }

        #region Animation Handling

        public void ChangeAnimationState(PlayerAnimatorState newState)
        {
            //Stop if currently played Animation matches attempted animation
            if (currentState == newState) return;
            
            //Play animation
            animator.Play(newState.ToString());
            
            //replace currentState
            currentState = newState;
        }

        #endregion

        #region Event Handling

        public void OnEnvironmentalTrapHit()
        {
            StartCoroutine(controller.ShortControlSuspension());
        }

        #endregion

        #region New scene transitions

        

        public void EnteringSceneAnimation(TransitionDirection transitionDirection, bool rightDrift)
        {
            if (transitionDirection == TransitionDirection.Up)
            {
                StartCoroutine(UpTransitionAnimation(rightDrift));
            } 
            else
            {
                var direction = transitionDirection == TransitionDirection.Right ? 1 : -1;
                StartCoroutine(SideTransitionAnimation(direction));
            }
        }

        private IEnumerator UpTransitionAnimation(bool rightDrift)
        {
            //Disables Controls
            movement.IgnoreRun = true;
            controller.DisableAllControls();
            
            //Checks if player goes right or left
            var force = rightDrift ? upAnimationForce : new Vector2(upAnimationForce.x * -1, upAnimationForce.y);
            
            //Adds Force
            rb.AddForce(force, ForceMode2D.Impulse);
            
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

public enum PlayerAnimatorState
{
    PlayerIdle,
    PlayerWalk,
    PlayerJump,
    PlayerJumpEnd,
    PlayerDeath,
    PlayerDash,
    PlayerAttack
}