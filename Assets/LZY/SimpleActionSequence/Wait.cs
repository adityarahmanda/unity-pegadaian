using System.Collections;
using LZY;
using LZY.SimpleActionSequence;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wait : SequenceAction, IActionWaitable
{
    public enum WaitType
    {
        Flat,
        Random
    }
    
    public bool IsDoneWaiting { get; private set; }
    
    [EnumToggleButtons] public WaitType waitType;
    [ShowIf(nameof(waitType), WaitType.Flat)] public float flatDuration = 1f;
    [ShowIf(nameof(waitType), WaitType.Random)] public float minDuration = 1f;
    [ShowIf(nameof(waitType), WaitType.Random)] public float maxDuration = 3f;
    
    public override void Execute()
    {
        CoroutineRunner.StartNewCoroutine(WaitCoroutine());
    }

    private IEnumerator WaitCoroutine()
    {
        IsDoneWaiting = false;
        float duration;
        if (waitType == WaitType.Random)
            duration = Random.Range(minDuration, maxDuration);
        else
            duration = flatDuration;
        yield return new WaitForSeconds(duration);
        IsDoneWaiting = true;
    }
}