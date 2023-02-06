using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

namespace _Core._5_Player.PlayerCamera
{
    public class FollowPlayerCamera : MonoBehaviour
    {
        private CinemachineConfiner2D cameraBorder;
        private GameObject player;
        private CinemachineVirtualCamera playerCamera;


        private void Awake()
        {
            playerCamera = GetComponent<CinemachineVirtualCamera>();
            cameraBorder = GetComponent<CinemachineConfiner2D>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            //Its important that the polygon collider is perfectly square shaped. Uneven areas will make the camera jitter around corners
            var levelBorder = GameObject.FindGameObjectWithTag("LevelBorder")?.GetComponent<PolygonCollider2D>();
            cameraBorder.m_BoundingShape2D = levelBorder;
        }

        private void LateUpdate()
        {
            if (!ReferenceEquals(player, null) && !player.IsDestroyed())
            {
                playerCamera.Follow = player.transform;
            }
        }
    }
}