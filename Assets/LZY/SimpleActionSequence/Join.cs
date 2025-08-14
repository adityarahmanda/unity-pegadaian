using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class Join : SequenceAction, IActionWaitable
    {
        public bool IsDoneWaiting { get; private set; }
    
        [SerializeReference] public List<SequenceAction> actions = new List<SequenceAction>();
    
        public override void Execute()
        {
            CoroutineRunner.StartNewCoroutine(JoinCoroutine());
        }

        private IEnumerator JoinCoroutine()
        {
            IsDoneWaiting = false;
        
            var actionCoroutines = new List<Coroutine>();
            foreach (var action in actions)
            {
                var coroutine = CoroutineRunner.StartNewCoroutine(ExecuteSequenceCoroutine(action));
                actionCoroutines.Add(coroutine);
            }
            
            foreach (var coroutine in actionCoroutines)
                yield return coroutine;
        
            IsDoneWaiting = true;
        }

        private IEnumerator ExecuteSequenceCoroutine(SequenceAction action)
        {
            if (action.delay > 0)
                yield return new WaitForSeconds(action.delay);
            
            action.Execute();
            
            if (action is IActionDuration durationAction)
                yield return new WaitForSeconds(durationAction.Duration);
            else if (action is IActionWaitable waitableAction)
                yield return new WaitUntil(() => waitableAction.IsDoneWaiting);
        }
    }
}