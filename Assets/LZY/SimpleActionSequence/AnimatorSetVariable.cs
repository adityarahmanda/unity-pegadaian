using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    [Serializable]
    public class AnimatorSetVariable : SequenceAction, IActionDuration
    {
        public enum VariableType
        {
            Bool = 0,
            Int = 1,
            Float = 2,
            Trigger = 3
        }

        public enum DurationType
        {
            Flat = 0,
            AnimationClip = 1,
        }

        public Animator animator;
        public VariableType variableType;
        public string variableName;

        [ShowIf(nameof(variableType), VariableType.Bool)]
        public bool boolValue;

        [ShowIf(nameof(variableType), VariableType.Int)]
        public int intValue;

        [ShowIf(nameof(variableType), VariableType.Float)]
        public float floatValue;

        [EnumToggleButtons] public DurationType durationType;

        [ShowIf(nameof(durationType), DurationType.Flat)]
        public float animationDuration;

        [ShowIf(nameof(durationType), DurationType.AnimationClip)]
        public AnimationClip animationClip;

        public float Duration => durationType == DurationType.Flat ? animationDuration : (animationClip == null ? 0 : animationClip.length);

        public override void Execute()
        {
            if (animator == null) return;

            switch (variableType)
            {
                case VariableType.Bool:
                    animator.SetBool(variableName, boolValue);
                    break;
                case VariableType.Int:
                    animator.SetInteger(variableName, intValue);
                    break;
                case VariableType.Float:
                    animator.SetFloat(variableName, floatValue);
                    break;
                case VariableType.Trigger:
                    animator.SetTrigger(variableName);
                    break;
            }
        }
    }
}
