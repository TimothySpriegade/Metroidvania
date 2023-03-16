using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss
{
    public class SpikeGround : MonoBehaviour
    {
        [SerializeField] private float spikeActivationDelay;
        private Collider2D[] spikeColliders;
        private Tween animationTween;

        private void Awake()
        {
            spikeColliders = GetComponentsInChildren<Collider2D>();
        }

        private void OnEnable()
        {
            animationTween = transform.DOMoveY(0.8f, spikeActivationDelay).SetEase(Ease.InExpo).SetRelative();
            
            DOVirtual.DelayedCall(spikeActivationDelay, () =>
            {
                foreach (var collider in spikeColliders)
                {
                    collider.enabled = true;
                }
            });
        }

        private void OnDisable()
        {
            animationTween?.Kill();
            transform.position = new Vector2(0, -2.4f);
            
            foreach (var collider in spikeColliders)
            {
                collider.enabled = false;
            }
        }
    }
}
