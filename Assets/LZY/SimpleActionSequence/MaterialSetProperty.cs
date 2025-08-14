using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    [Serializable]
    public class MaterialSetProperty : SequenceAction, IActionDuration
    {
        public enum PropertyType
        {
            Bool = 0,
            Int = 1,
            Float = 2,
            Color = 3
        }
        
        public enum MaterialSelectionType
        {
            Material = 0,
            Renderer = 1
        }

        public MaterialSelectionType materialSelectionType;
        
        [ShowIf(nameof(materialSelectionType), MaterialSelectionType.Material)]
        public Material material;
        
        [ShowIf(nameof(materialSelectionType), MaterialSelectionType.Renderer)]
        public Renderer renderer;
        
        [ShowIf(nameof(materialSelectionType), MaterialSelectionType.Renderer)]
        public int materialIndex = 0;

        public string propertyId;
        public PropertyType propertyType;

        [ShowIf(nameof(propertyType), PropertyType.Bool)]
        public bool boolValue;

        [ShowIf(nameof(propertyType), PropertyType.Int)]
        public int intValue;

        [ShowIf(nameof(propertyType), PropertyType.Float)]
        public float floatValue;
        
        [ShowIf(nameof(propertyType), PropertyType.Color)]
        public Color colorValue;

        [ShowIf(nameof(PropertyHasLerp), PropertyType.Float)]
        public float lerpDuration;
        
        [ShowIf(nameof(PropertyHasLerp), PropertyType.Float)]
        public Ease lerpEase;
        public float Duration => PropertyHasLerp() ? lerpDuration : 0;

        private bool PropertyHasLerp()
        {
            switch (propertyType)
            {
                case PropertyType.Float:
                case PropertyType.Color:
                    return true;
                default:
                    return false;
            }
        }

        public override void Execute()
        {
            Material target = null;
            switch (materialSelectionType)
            {
                case MaterialSelectionType.Material:
                    target = material;
                    break;
                case MaterialSelectionType.Renderer:
                    if (materialIndex >= 0 && materialIndex < renderer.materials.Length)
                        target = renderer.materials[materialIndex];
                    break;
            }

            if (target == null) return;

            switch (propertyType)
            {
                case PropertyType.Bool:
                    target.SetInteger(propertyId, boolValue ? 1 : 0);
                    break;
                case PropertyType.Int:
                    target.SetInteger(propertyId, intValue);
                    break;
                case PropertyType.Float:
                    target.DOFloat(floatValue, propertyId, lerpDuration).SetEase(lerpEase);
                    break;
                case PropertyType.Color:
                    target.DOColor(colorValue, propertyId, lerpDuration).SetEase(lerpEase);
                    break;
            }
        }
    }
}
