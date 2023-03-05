using _Framework;
using DG.Tweening;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerCheckpointController : MonoBehaviour
    {
        private Vector2 checkpointPosition;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            DOVirtual.DelayedCall(0.2f, () => checkpointPosition = transform.position, false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("EnvironmentalTrapCheckpoint") &&
                !col.transform.position.Equals(checkpointPosition))
            {
                var colliderPosition = col.transform.position;
                this.Log($"Assigning new checkpoint: {colliderPosition}");
                checkpointPosition = colliderPosition;
            }
        }

        public void ResetPlayerPosition()
        {
            this.Log($"Resetting Player to {checkpointPosition}");
            transform.position = checkpointPosition;
            rb.velocity = Vector2.zero;
        }
    }
}