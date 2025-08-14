using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    [Serializable]
    public class Sequence : IDisposable
    {
        [SerializeReference] public List<SequenceAction> actionList = new List<SequenceAction>();
        private Coroutine _playCoroutineSequence;

        public Coroutine ExecuteSequence()
        {
            StopSequence();
            _playCoroutineSequence = CoroutineRunner.StartNewCoroutine(ExecuteSequenceCoroutine());
            return _playCoroutineSequence;
        }

        public void StopSequence()
        {
            if (_playCoroutineSequence != null)
                CoroutineRunner.StopRunningCoroutine(_playCoroutineSequence);
        }

        private IEnumerator ExecuteSequenceCoroutine()
        {
            foreach (var action in actionList)
            {
                if (action == null) continue;

                if (action.delay > 0)
                    yield return new WaitForSeconds(action.delay);
                
                action.Execute();

                if (action is IActionDuration durationAction)
                    yield return new WaitForSeconds(durationAction.Duration);
                else if (action is IActionWaitable waitableAction)
                    yield return new WaitUntil(() => waitableAction.IsDoneWaiting);
            }
        }
        
        public T GetActionSequence<T>() where T : SequenceAction => actionList.OfType<T>().FirstOrDefault();

        public T[] GetActionSequences<T>() where T : SequenceAction => actionList.OfType<T>().ToArray();

        public void Dispose()
        {
            StopSequence();
        }
    }

    [Serializable]
    public abstract class SequenceAction
    {
        public float delay;
        public abstract void Execute();
    }

    public interface IActionWaitable
    {
        bool IsDoneWaiting { get; }
    }

    public interface IActionDuration
    {
        float Duration { get; }
    }
}
