using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LZY.Pegadaian.Firebase;
using LZY.Pegadaian.Misc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.Pegadaian.ZonaCerdas
{
    public class ZonaCerdasCore : SceneCore
    {
        [Header("Read Only")]
        [ShowInInspector, ReadOnly] private Dictionary<string, Message> _messages = new Dictionary<string, Message>();
        [ShowInInspector, ReadOnly] private Queue<MessageView> _queueMessageViews = new Queue<MessageView>();

        [Header("Message Placement Settings")]
        [SerializeField] private List<MessageView> messagesViews = new List<MessageView>();

        private FirebaseService _firebaseService;
        private readonly UniTaskQueue _messageTaskQueue = new UniTaskQueue();
        
        private long _lastMessageCreated;
        private bool _isReady;

        protected override void OnPostInitialize()
        {
            _firebaseService = GetService<FirebaseService>();
            _ = _firebaseService.ListenToMessages(OnFirebaseMessageUpdate, OnFirebaseMessageUpdate, Debug.LogError);
            PopulateMessagesViews().Forget();
        }

        protected override void OnPostDeinitialize()
        {
            _messageTaskQueue?.Dispose();
        }

        private void ClearMessagesViews()
        {
            _queueMessageViews.Clear();
            foreach (var messageView in messagesViews)
            {
                messageView.Reset();
                _queueMessageViews.Enqueue(messageView);                
            }
        }
        
        private async UniTask PopulateMessagesViews()
        {
            _messages.Clear();
            ClearMessagesViews();
            var messages = await _firebaseService.GetMessagesAsync(messagesViews.Count);
            foreach (var (messageId, messageData) in messages)
                _messageTaskQueue.QueueTask(async token => await AddMessageAsync(messageId, messageData, token));
        }

        private void OnFirebaseMessageUpdate(RealtimeDatabase_SSEData data)
        {
            if (data.Path != "/")
            {
                var path = data.Path.Trim('/');
                var parts = path.Split('/');

                if (parts.Length == 1)
                {
                    // /<messageId>
                    var messageId = parts[0];
                    if (data.Data == null)
                        _messageTaskQueue.QueueTask(async token => await RemoveMessageAsync(messageId, token));
                    else
                    {
                        var messageData = Utils.CreateMessage(data.Data);
                        _messageTaskQueue.QueueTask(async token => await AddMessageAsync(messageId, messageData, token));
                    }
                }
                else if (parts.Length == 2)
                {
                    // /<messageId>/<field>
                    var messageId = parts[0];
                    var fieldName = parts[1];
                    var fieldValue = data.Data?.ToString();
                    
                    var messageView = _queueMessageViews.FirstOrDefault(x => x.Id == messageId);
                    if (messageView != null) 
                        messageView.UpdateMessageField(fieldName, fieldValue);
                }
                // else: support deeper nesting if needed
            }
        }
       
        [Button]
        public void AddDummyMessage()
        {
            AddMessage(Guid.NewGuid().ToString(), new Message()
            {
                author = "Dummy",
                message = "Dummy Message",
                createdAt = _lastMessageCreated,
                isApproved = true
            });
        }
        
        private void AddMessage(string messageId, Message message)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                Debug.LogWarning("TryAddMessage failed! Null or empty message key: " + messageId);
                return;
            }

            if (_queueMessageViews.Any(x => x.Id == messageId))
            {
                Debug.LogWarning("TryAddMessage failed! Message id already exist: " + messageId);
                return;
            }
            
            if (message.createdAt < _lastMessageCreated) return;

            _messageTaskQueue.QueueTask(async token => await AddMessageAsync(messageId, message, token));
        }

        private async UniTask AddMessageAsync(string messageId, Message message, CancellationToken token = default)
        {
            if (_messages.ContainsKey(messageId))
            {
                Debug.Log("Updating message: " + messageId);
                _messages[messageId] = message;
                
                var messageView = _queueMessageViews.FirstOrDefault(x => x.Id == messageId);
                if (messageView == null) return;
                
                await SetMessageViewAsync(messageView, messageId, message, token);
            }
            else
            {
                Debug.Log("Adding new message: " + messageId);
                _messages.Add(messageId, message);
                
                if (!_queueMessageViews.TryDequeue(out var messageView)) return;
                _queueMessageViews.Enqueue(messageView);

                await SetMessageViewAsync(messageView, messageId, message, token);
            }
        }

        private async UniTask RemoveMessageAsync(string messageId, CancellationToken token = default)
        {
            Debug.Log("Removing message: " + messageId);
            
            var messageView = _queueMessageViews.FirstOrDefault(x => x.Id == messageId);
            if (messageView != null)
                messageView.Reset();
            _messages.Remove(messageId);

            var oldMessagePair = _messages.FirstOrDefault(kvp => !_queueMessageViews.Any(x => x.Id == kvp.Key));
            if (string.IsNullOrEmpty(oldMessagePair.Key) || oldMessagePair.Value == null) return;
            
            await SetMessageViewAsync(messageView, oldMessagePair.Key, oldMessagePair.Value, token);
        }
        
        private async UniTask SetMessageViewAsync(MessageView messageView, string messageId, Message message, CancellationToken token = default)
        {
            await FirebaseImageLoader.FetchImageAsync(message.imageUrl);
            await messageView.PlayExitAnimation(token);
            await messageView.PlayEnterAnimation(messageId, message, token);
        }
    }
}
