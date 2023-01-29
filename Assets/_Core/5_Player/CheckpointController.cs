using UnityEngine;

namespace _Core._5_Player
{
    public class CheckpointController : MonoBehaviour
    {
        private Vector2 checkpointPosition;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("EnvironmentalTrapCheckpoint")) checkpointPosition = col.transform.position;
        }

        public void ResetPlayerPosition()
        {
            transform.position = checkpointPosition;
            rb.velocity = Vector2.zero;
        }
    }
}