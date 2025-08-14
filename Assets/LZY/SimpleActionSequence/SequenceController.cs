using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public class SequenceController : MonoBehaviour
    {
        public Sequence Sequence => sequence;
        public List<SequenceAction> ActionList => sequence.actionList;

        [SerializeField, HideLabel] private Sequence sequence;
        
        [Button]
        public Coroutine ExecuteSequence()
        {
            return sequence.ExecuteSequence();
        }

        [Button]
        public void StopSequence()
        {
            sequence.StopSequence();
        }
        
        public T GetActionSequence<T>() where T : SequenceAction => sequence.GetActionSequence<T>();

        public T[] GetActionSequences<T>() where T : SequenceAction => sequence.GetActionSequences<T>();
    }
}
