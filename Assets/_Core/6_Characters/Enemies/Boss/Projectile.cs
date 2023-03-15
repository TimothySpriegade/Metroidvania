using DG.Tweening;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float rotateDuration;
        [SerializeField] private float moveDuration;
        public float timeToLive;
        
        private Tween spinTween;
        private Tween moveTween;
        private Tween killTween;

        private void Start()
        {
            var target = new Vector3(0, 0, 360);
            spinTween = transform.DORotate(target, rotateDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetRelative()
                .SetLoops(-1);

            moveTween = transform.DOMoveX(-50, moveDuration)
                .SetEase(Ease.Linear)
                .SetRelative();

            killTween = DOVirtual.DelayedCall(timeToLive, () => Destroy(gameObject));
        }

        private void OnDisable()
        {
            spinTween?.Kill();
            moveTween?.Kill();
            killTween?.Kill();
        }
    }
}
