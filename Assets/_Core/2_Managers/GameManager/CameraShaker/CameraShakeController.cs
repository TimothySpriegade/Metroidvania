using _Framework.SOEventSystem;
using Cinemachine;
using UnityEngine;

namespace _Core._2_Managers.GameManager.CameraShaker
{
    public class CameraShakeController : MonoBehaviour
    {
        private CinemachineImpulseSource shake;

        private void Awake()
        {
            shake = GetComponent<CinemachineImpulseSource>();
        }

        public void ShakeCamera(CameraShakeConfiguration config)
        {
            shake.m_ImpulseDefinition.m_AmplitudeGain = config.strength;
            shake.m_ImpulseDefinition.m_FrequencyGain = config.speed;
            shake.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = config.length;

            shake.GenerateImpulse();
        }
    }
}
