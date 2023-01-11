using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Player.PlayerCamera
{
    public class PlayerCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera playerCamera;
        private GameObject player;
        private CinemachineBasicMultiChannelPerlin cameraShake;
        private float customTime = 1;

        private void Awake()
        {
            playerCamera = GetComponent<CinemachineVirtualCamera>();
            cameraShake = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (!ReferenceEquals(player, null))
            {
                playerCamera.Follow = player.transform;
            }
        }

        public void SetCustomTime(float time)
        {
            customTime = time;
        }

        public void ShakeCameraInitiator(float intensity)
        {
            StartCoroutine(ShakeCamera(intensity, customTime));
        }
        
        private IEnumerator ShakeCamera(float intensity, float time)
        {
            cameraShake.m_AmplitudeGain = intensity;
            while (cameraShake.m_AmplitudeGain >= 0)
            {
                cameraShake.m_AmplitudeGain -= (intensity * Time.deltaTime) / time;
                yield return new WaitForEndOfFrame();
            }

            cameraShake.m_AmplitudeGain = 0;
            customTime = 1;
        }
    }

}