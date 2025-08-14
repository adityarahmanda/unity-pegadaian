using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class GameObjectSetActive : SequenceAction
    {
        public GameObject gameObjectRef;
        public bool value = true;

        public override void Execute()
        {
            if (gameObjectRef == null) return;

            gameObjectRef.SetActive(value);
        }
    }
}
