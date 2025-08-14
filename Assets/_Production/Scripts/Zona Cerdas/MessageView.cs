using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LZY.SimpleActionSequence;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.Pegadaian.ZonaCerdas
{
    public class MessageView : MonoBehaviour
    {
        public string Id => messageUI.Id;
        
        [SerializeField] private MessageUIController messageUI;
        [SerializeField] private SequenceController enterSequence;
        [SerializeField] private SequenceController exitSequence;
        [SerializeField] private SequenceController resetSequence;

        private void Awake()
        {
            Reset();
        }

        public void UpdateMessageField(string fieldName, string fieldValue)
        {
            messageUI.SetMessageField(fieldName, fieldValue);
        }
        
        [Button]
        public async UniTask PlayEnterAnimation(string messageId, Message message, CancellationToken token = default)
        {
            UnplayAllAnimation();

            messageUI.SetMessage(messageId, message);
            await enterSequence.ExecuteSequenceAsync();
        }

        [Button]
        public async UniTask PlayExitAnimation(CancellationToken token = default)
        {
            UnplayAllAnimation();
            await exitSequence.ExecuteSequenceAsync();
        }

        public void Reset()
        {
            messageUI.SetMessageEmpty();
            resetSequence.ExecuteSequence();
        }

        private void UnplayAllAnimation()
        {
            enterSequence.StopSequence();
            exitSequence.StopSequence();
        }
    }
}