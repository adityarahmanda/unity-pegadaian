using UnityEngine.Events;

namespace LZY.SimpleActionSequence
{
    public class Callback : SequenceAction
    {
        public UnityEvent callbacks;

        public override void Execute()
        {
            callbacks.Invoke();
        }
    }
}
