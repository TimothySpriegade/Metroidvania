using UnityEngine;

namespace Player.PlayerCamera
{
    public class PlayerCamera : MonoBehaviour
    {
        private Vector3 playerPos;
        [SerializeField] private GameObject player;


        private void FollowPlayer()
        {
            playerPos = player.transform.position;
            playerPos = Vector3.Lerp(transform.position, playerPos, 10f * Time.deltaTime);
            transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        }

        private void LateUpdate()
        {
            FollowPlayer();
        }
    }
}