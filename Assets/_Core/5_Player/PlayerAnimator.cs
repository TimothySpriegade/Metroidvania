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
        private SpriteRenderer spriteRenderer;
        private PlayerMovement movement;
        private PlayerController controller;
        private Rigidbody2D rb;

        #endregion

        #region Animation Vars

        [SerializeField] private Vector2 upAnimationForce;
        private bool collisionDetected;
        private bool disabledAnimation;
        private float animationBlock;

        private PlayerAnimatorState currentState;

        #endregion
        
        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = spriteRenderer.GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<PlayerMovement>();
            controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            animationBlock -= Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            collisionDetected = true;
        }

        #region Animation Handling

        public void ChangeAnimationState(PlayerAnimatorState newState)
        {
            //Stop if currently played Animation matches attempted animation
            if (currentState == newState || animationBlock >= 0 || disabledAnimation) return;
            
            //Play animation
            animator.Play(newState.ToString());
            
            switch (newState)
            {
                case PlayerAnimatorState.PlayerLand:
                    //Land FX
                    animationBlock = 0.1f;
                    break;
                case PlayerAnimatorState.PlayerJump:
                    //Jump FX
                    break;
                case PlayerAnimatorState.PlayerDeath:
                    //DeathFX
                    disabledAnimation = true;
                    break;
            }
            
            //replace currentState
            currentState = newState;
        }

        public IEnumerator DashAnimation(Vector2 direction, float length, bool isFacingRight)
        {
            var startTime = Time.time;

            var degrees = 0;
            //Rotation depending on the direction
            if (direction.y != 0)
            {
                degrees = direction.x == 0 ? 90 : 45;
                if (direction.y > 0) degrees *= -1;
            }
            //Weird Euler shenanigans
            if (isFacingRight) degrees *= -1;

            //Applying Rotation
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, degrees);

            //Playing Animation
            animator.Play(PlayerAnimatorState.PlayerDash.ToString());
            currentState = PlayerAnimatorState.PlayerDash;

            //Keeping Rotation for Dash length
            while (Time.time - startTime < length + 0.1f)
            {
                yield return null;
            }

            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        #endregion

        
        #region Event Handling

        public void OnEnvironmentalTrapHit()
        {
            StartCoroutine(controller.ShortControlSuspension());
        }

        public void OnDeath()
        {
            ChangeAnimationState(PlayerAnimatorState.PlayerDeath);
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
    PlayerAttack,
    PlayerLand
}