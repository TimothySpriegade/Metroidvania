using UnityEngine;

namespace Player.PlayerCamera
{
    public class PlayerCamera : MonoBehaviour
    {
        private Vector3 playerPos;
        [SerializeField] private GameObject player;


        void FollowPlayer()
        {
            playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        }

        void LateUpdate()
        {
            FollowPlayer();
        }
    }
}