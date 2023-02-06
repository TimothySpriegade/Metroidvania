using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Core._3_Art_and_FX.Lights.AbilityLight
{
    public class AbilityLight : MonoBehaviour
    {
        private Light2D localLight;

        [SerializeField] private float speed;

        private void Awake()
        {
            localLight = GetComponent<Light2D>();
        }

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - speed * Time.deltaTime);
            //transform.DORotate(new Vector3(0,0,360), speed, RotateMode.Fast).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear); TODO uncomment
        }
    }
}
