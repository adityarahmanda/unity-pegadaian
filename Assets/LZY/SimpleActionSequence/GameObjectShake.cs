using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class GameObjectShake : SequenceAction, IActionDuration
    {
        public enum ValueType
        {
            Position = 0,
            Rotation,
            Scale
        }
        
        public Transform transform;
        public ValueType valueType;
        public Vector3 strength = Vector3.one;
        public int vibrato = 10;
        private float randomness = 90f;
        [ShowIf(nameof(valueType), ValueType.Position)]
        public bool snapping = false;
        public bool fadeOut = true;
        public ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full;
        public float duration;

        public float Duration => duration;

        public override void Execute()
        {
            if (transform == null) return;

            switch (valueType)
            {
                case ValueType.Position:
                    transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut, randomnessMode);
                    break;
                case ValueType.Rotation:
                    transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut, randomnessMode);
                    break;
                case ValueType.Scale:
                    transform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut, randomnessMode);
                    break;
            }
        }
    }
}
