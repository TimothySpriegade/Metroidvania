using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.NPC
{
    public class Cat : MonoBehaviour
    {
        private CatAnimatorState currentState;
        
        private Animator animator;

        private bool isFacingRight;

        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private Vector2 wallCheckSize;
        [SerializeField] private LayerMask groundLayer;
        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            DOVirtual.DelayedCall(1, RandomAnimation).SetLoops(-1);
        }

        private void MoveCat()
        {
            float direction;
            Debug.Log(Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, groundLayer));
            if (Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, groundLayer))
            {
                direction = isFacingRight ? -moveSpeed : moveSpeed;
            }
            else
            {
                direction = Random.value < 0.5f ? -moveSpeed : moveSpeed;
            }

            CheckDirectionToFace(direction > 0);
            transform.DOMoveX(direction, 1).SetRelative();
        }
        private void RandomAnimation()
        {
            var randomAnimation = Random.value < 0.3f ? CatAnimatorState.CatRun : (CatAnimatorState)Random.Range(0, 5);
            Debug.Log(randomAnimation);
            ChangeAnimationState(randomAnimation);
        }
        private void ChangeAnimationState(CatAnimatorState newState)
        {
            //Stop if currently played Animation matches attempted animation
            if (currentState == newState && newState != CatAnimatorState.CatRun) return;

            //Play animation
            animator.Play(newState.ToString());

            if (newState == CatAnimatorState.CatRun)
            {
                MoveCat();
            }
            
            //replace currentState
            currentState = newState;
        }

        #region Flip

        private void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != isFacingRight)
            {
                Flip();
            }
        }
        private void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }

        #endregion
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
        }
    }
}

public enum CatAnimatorState
{
    CatIdle1,
    CatIdle2,
    CatIdle3,
    CatSleep,
    CatRun,
}