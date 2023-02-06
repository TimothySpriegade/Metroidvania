using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Core._8_Environment.Torch
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private float amount;	//The amount of light flicker
        [SerializeField] private float speed;		//The speed of the flicker

        private Light2D localLight;
        private float intensity;
        private float offset;

        private void Awake()
        {
            //Get a reference to the Light component on the child game object
            localLight = GetComponentInChildren<Light2D>();

            //Record the intensity and pick a random seed number to start
            intensity = localLight.intensity;
            offset = Random.Range(0, 10000);
        }

        private void Update ()
        {
            //Using perlin noise, determine a random intensity amount
            var noiseAmount = Mathf.PerlinNoise(Time.time * speed + offset, Time.time * speed + offset) * amount;
            localLight.intensity = intensity + noiseAmount;
        }
    }
}