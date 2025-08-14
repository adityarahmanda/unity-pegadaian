using System;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    [Serializable]
    public class AnimationPlay : SequenceAction
    {
        public Animation animation;

        public override void Execute()
        {
            if (animation == null) return;
            animation.Play();
        }
    }
}
