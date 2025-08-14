using System.Collections;

namespace LZY.SimpleActionSequence
{
    public class CoroutineSequencePlay : SequenceAction, IActionWaitable
    {
        public CoroutineSequence coroutineSequence;

        public override void Execute()
        {
            CoroutineRunner.StartNewCoroutine(PlayCoroutineSequenceCoroutine());
        }

        public IEnumerator PlayCoroutineSequenceCoroutine()
        {
            IsDoneWaiting = false;
            yield return coroutineSequence.ExecuteSequence();
            IsDoneWaiting = true;
        }

        public bool IsDoneWaiting { get; private set; }
    }
}
