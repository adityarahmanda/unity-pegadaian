using System.Collections;
using UnityEngine;

namespace LZY.SimpleActionSequence
{
    public abstract class CoroutineSequence : MonoBehaviour
    {
        public Coroutine ExecuteSequence()
        {
            return StartCoroutine(SequenceCoroutine());
        }

        protected abstract IEnumerator SequenceCoroutine();
    }
}
