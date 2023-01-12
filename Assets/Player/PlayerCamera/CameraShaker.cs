using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Player.PlayerCamera
{
    public class CameraShaker : MonoBehaviour
    {
        private CinemachineBasicMultiChannelPerlin cameraShake;
        private float customTime = 1;
        private CinemachineVirtualCamera playerCamera;
        
        private void Awake()
        {
            cameraShake = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            playerCamera = GetComponent<CinemachineVirtualCamera>();
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
