using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class SpriteRendererSetSprite : SequenceAction
    {
        public SpriteRenderer renderer;
        public Sprite sprite;

        public override void Execute()
        {
            if (renderer == null) return;
            
            renderer.sprite = sprite;
        }
    }
}
