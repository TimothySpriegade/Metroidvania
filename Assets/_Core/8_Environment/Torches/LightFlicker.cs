using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Core._8_Environment.Torches
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private float amount;
        [SerializeField] private float speed;

        private Light2D localLight;
        private float intensity;
        private float offset;

        private void Awake()
        {
            localLight = GetComponentInChildren<Light2D>();

            intensity = localLight.intensity;
            offset = Random.Range(0, 10000);
        }

        private void Update()
        {
            var noiseAmount = Mathf.PerlinNoise(Time.time * speed + offset, Time.time * speed + offset) * amount;
            localLight.intensity = intensity + noiseAmount;
        }
    }
}