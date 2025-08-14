using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    [Serializable]
    public class Particle : SequenceAction, IActionDuration
    {
        public enum ParticleActionType
        {
            Play,
            Stop
        }
        
        public enum DurationType
        {
            Flat,
            ParticleDuration
        }
        
        public ParticleSystem particleSystem;
        
        [EnumToggleButtons]
        public ParticleActionType actionType = ParticleActionType.Play;
        
        public bool withChildren = true;
        
        [ShowIf(nameof(actionType), ParticleActionType.Stop)]
        public ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmittingAndClear;

        [EnumToggleButtons]
        [ShowIf(nameof(actionType), ParticleActionType.Play)]
        public DurationType durationType;

        [ShowIf(nameof(InspectorShowDuration))]
        public float duration;

        public override void Execute()
        {
            CoroutineRunner.StartNewCoroutine(ParticleCoroutine());
        }

        private IEnumerator ParticleCoroutine()
        {
            if (particleSystem != null)
            {
                if (actionType == ParticleActionType.Play)
                {
                    particleSystem.Play(withChildren);
                    yield return new WaitForSeconds(Duration);
                }
                else
                    particleSystem.Stop(withChildren, stopBehavior);
            }
            
        }

        public float Duration
        {
            get
            {
                if (actionType == ParticleActionType.Stop)
                    return 0;
                
                if (durationType == DurationType.Flat)
                    return duration;
                else
                    return particleSystem.main.duration;
            }
        }

        private bool InspectorShowDuration => actionType == ParticleActionType.Play && durationType == DurationType.Flat;
    }
}
