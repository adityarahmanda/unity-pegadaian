using System.Collections;

namespace LZY.SimpleActionSequence
{
    public class SequenceControllerExecute : SequenceAction, IActionWaitable
    {
        public bool IsDoneWaiting { get; set;  }

        public SequenceController sequence;
    
        public override void Execute()
        {
            if (sequence == null) return;

            CoroutineRunner.StartNewCoroutine(ExecuteSequenceCoroutine());
        }

        private IEnumerator ExecuteSequenceCoroutine()
        {
            IsDoneWaiting = false;
            yield return sequence.ExecuteSequence();
            IsDoneWaiting = true;
        }
    }
}