using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class LightSetColor : SequenceAction, IActionDuration
    {
        public Light light;
        public Color color;
        public bool useColorTemperature;
        [ShowIf(nameof(useColorTemperature))]
        public float colorTemperature = 6500f;
        public float duration;
        public Ease ease;
        
        public float Duration => duration;

        public override void Execute()
        {
            light.useColorTemperature = useColorTemperature;
            DOTween.To(() => light.colorTemperature, x => light.colorTemperature = x, colorTemperature, duration).SetEase(ease);
            light.DOColor(color, duration).SetEase(ease);
        }
    }
}
