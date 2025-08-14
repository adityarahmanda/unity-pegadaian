using System;
using LZY.SimpleActionSequence;
using Sirenix.OdinInspector;

namespace LZY.SimpleAudioManager
{
    [Serializable]
    public class SFXPlay : SequenceAction
    {
        public enum PlayMethod
        {
            ById = 0,
            ByClip,
        }

        public PlayMethod method;
        
        [ShowIf(nameof(method), PlayMethod.ById)]
        public string id;
        
        [ShowIf(nameof(method), PlayMethod.ByClip)]
        public AudioData clipData;

        public override void Execute()
        {
            if (method == PlayMethod.ById)
            {
                AudioManager.PlaySFX(id);
            }
            
            if (method == PlayMethod.ByClip)
            {
                AudioManager.PlaySFX(clipData);
            }
        }
    }
}
