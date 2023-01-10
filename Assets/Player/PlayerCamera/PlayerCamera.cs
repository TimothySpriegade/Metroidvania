using System;
using UnityEngine;
using Cinemachine;

namespace Player.PlayerCamera
{
    public class PlayerCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera playerCamera;
        private GameObject player;
        private void Start()
        {
            playerCamera = GetComponent<CinemachineVirtualCamera>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (!ReferenceEquals(player, null))
            {
                playerCamera.Follow = player.transform;
            }
        }
    }

}