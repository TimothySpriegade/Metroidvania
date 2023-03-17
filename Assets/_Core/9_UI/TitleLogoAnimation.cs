using DG.Tweening;
using UnityEngine;

namespace _Core._9_UI
{
    public class TitleLogoAnimation : MonoBehaviour
    {
        [SerializeField] private float amount;
        [SerializeField] private float duration;

        [SerializeField] private float sideAmount;
        [SerializeField] private float sideDuration;
        
        private Tween animationTween;
        private Tween sidewardsMotionTween;

        private void Start()
        {
            animationTween = transform.DOMoveY(amount, duration)
                .SetEase(Ease.InOutSine)
                .SetRelative()
                .SetLoops(-1, LoopType.Yoyo);

            sidewardsMotionTween = DOTween.Sequence()
                .Append(CreateSideTween(sideAmount))
                .Append(CreateSideTween(-2*sideAmount))
                .SetEase(Ease.Linear)
                .SetRelative()
                .SetLoops(-1, LoopType.Yoyo);
        }

        private Tween CreateSideTween(float tweenAmount)
        {
            return transform.DOMoveX(tweenAmount, sideDuration).SetEase(Ease.InOutSine);
        }
        

        private void OnDisable()
        {
            animationTween?.Kill();
            sidewardsMotionTween?.Kill();
        }
    }
}
