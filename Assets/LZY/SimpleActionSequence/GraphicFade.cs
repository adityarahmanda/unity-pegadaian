using System;
using DG.Tweening;
using UnityEngine.UI;

namespace LZY.SimpleActionSequence
{
    public class GraphicFade : SequenceAction, IActionDuration
    {
        public Graphic graphic;
        public float value;
        public float duration;
        public Ease ease;

        public float Duration => duration;

        public override void Execute()
        {
            if (graphic == null) return;
            
            graphic.DOFade(value, duration).SetEase(ease);
        }
    }
}
