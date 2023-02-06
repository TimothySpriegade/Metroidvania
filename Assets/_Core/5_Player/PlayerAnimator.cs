using System.Collections;
using _Core._2_Managers.GameManager.PlayerSpawner;
using UnityEngine;

namespace _Core._5_Player
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
            while (Time.time - startTime < length + 0.1f) yield return null;

            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        #endregion


        #region Event Handling

        public void OnDeath()
        {
            ChangeAnimationState(PlayerAnimatorState.PlayerDeath);
        }

        #endregion

        #region New scene transitions

        public void EnteringSceneAnimation(TransitionDirection transitionDirection, bool rightDrift)
        {
            switch (transitionDirection)
            {
                case TransitionDirection.Up:
                    StartCoroutine(UpTransitionAnimation(rightDrift));
                    return;
                case TransitionDirection.Down:
                    StartCoroutine(DownTransitionAnimation());
                    return;
                default:
                {
                    var direction = transitionDirection == TransitionDirection.Right ? 1 : -1;
                    StartCoroutine(SideTransitionAnimation(direction));
                    break;
                }
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
            while (!collisionDetected) yield return null;

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

        private IEnumerator DownTransitionAnimation()
        {
            //Disables Controls
            controller.DisableAllControls();

            var startTime = Time.time;

            //Sets player velocity sidewards for .6 seconds
            while (Time.time - startTime <= 0.6f)
            {
                yield return null;
            }

            //Activates Controls
            controller.EnableAllControls();
        }

        #endregion
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
}