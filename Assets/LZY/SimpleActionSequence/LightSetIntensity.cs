using System;
using DG.Tweening;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class LightSetIntensity : SequenceAction, IActionDuration
    {
        public Light light;
        public float value;
        public float duration;
        public Ease ease;
        public float Duration => duration;

        public override void Execute()
        {
            light.DOIntensity(value, duration).SetEase(ease);
        }
    }
}
