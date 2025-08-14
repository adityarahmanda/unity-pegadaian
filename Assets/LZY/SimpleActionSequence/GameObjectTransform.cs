using DG.Tweening;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class GameObjectTransform : SequenceAction, IActionDuration
    {
        public enum ValueType
        {
            Position = 0,
            LocalPosition,
            Rotation,
            LocalRotation,
            Scale
        }
        
        public Transform transform;
        public ValueType valueType;
        public Vector3 value;
        public float duration;
        public Ease ease;

        public float Duration => duration;

        public override void Execute()
        {
            if (transform == null) return;

            Tween tween = null;
            switch (valueType)
            {
                case ValueType.Position:
                    tween = transform.DOMove(value, duration);
                    break;
                case ValueType.LocalPosition:
                    tween = transform.DOLocalMove(value, duration);
                    break;
                case ValueType.Rotation:
                    tween = transform.DORotate(value, duration);
                    break;
                case ValueType.LocalRotation:
                    tween = transform.DOLocalRotate(value, duration);
                    break;
                case ValueType.Scale:
                    tween = transform.DOScale(value, duration);
                    break;
            }
            
            if (tween != null)
                tween.SetEase(ease);
        }
    }
}
