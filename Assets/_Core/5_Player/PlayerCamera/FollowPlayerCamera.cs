using _Core._10_Utils;
using _Core._5_Player.ScriptableObjects;
using Cinemachine;
using UnityEngine;

namespace _Core._5_Player.PlayerCamera
{
    public class FollowPlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerReferenceData playerData;
        private CinemachineConfiner2D cameraBorder;
        private CinemachineVirtualCamera playerCamera;


        private void Awake()
        {
            playerCamera = GetComponent<CinemachineVirtualCamera>();
            cameraBorder = GetComponent<CinemachineConfiner2D>();
        }

        private void Start()
        {
            //Its important that the polygon collider is perfectly square shaped. Uneven areas will make the camera jitter around corners
            var levelBorder = GameObject.FindGameObjectWithTag("LevelBorder")?.GetComponent<PolygonCollider2D>();
            cameraBorder.m_BoundingShape2D = levelBorder;
        }

        private void LateUpdate()
        {
            if (TargetUtils.TargetExists(playerData.player))
            {
                playerCamera.Follow = playerData.player.transform;
            }
        }
    }
}