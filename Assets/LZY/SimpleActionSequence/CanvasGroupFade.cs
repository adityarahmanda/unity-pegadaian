using DG.Tweening;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class CanvasGroupFade : SequenceAction, IActionDuration
    {
        public CanvasGroup canvasGroup;
        public float value;
        public float duration;
        public Ease ease;

        public float Duration => duration;

        public override void Execute()
        {
            if (canvasGroup == null) return;
            
            canvasGroup.DOFade(value, duration).SetEase(ease);
        }
    }
}
