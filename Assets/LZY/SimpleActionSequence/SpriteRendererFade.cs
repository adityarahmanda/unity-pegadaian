using System;
using DG.Tweening;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class SpriteRendererFade : SequenceAction, IActionDuration
    {
        public SpriteRenderer renderer;
        public float value;
        public float duration;
        public Ease ease;

        public float Duration => duration;

        public override void Execute()
        {
            if (renderer == null) return;
            
            renderer.DOFade(value, duration).SetEase(ease);
        }
    }
}
