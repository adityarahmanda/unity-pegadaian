using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LZY.Pegadaian.Firebase;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace LZY.Pegadaian.ZonaCerdas
{
    public class FirebaseService : SceneService
    {
        protected override string GetId() => nameof(FirebaseService);

        [SerializeField] private FirebaseSettings firebaseSettings;

        public async UniTask<Dictionary<string, Message>> GetMessagesAsync(int messagesCount = -1, CancellationToken cancellationToken = default)
        {
            var url = $"{firebaseSettings.GetDatabaseBaseUrl()}/{firebaseSettings.MessagesEndpoint}.json?orderBy=\"isApproved\"&equalTo=true&limitToLast={messagesCount}";
            Debug.Log("GetMessagesAsync: " + url);
            
            var request = UnityWebRequest.Get(url);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                    return null;
                await Task.Yield();
            }

            var json = request.downloadHandler.text; 
            Debug.Log(json);
            
            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception($"Error: {request.error}");
            
            return JsonConvert.DeserializeObject<Dictionary<string, Message>>(json);
        }
        
        public async Task ListenToMessages(UnityAction<RealtimeDatabase_SSEData> onPut = null, UnityAction<RealtimeDatabase_SSEData> onPatch = null, UnityAction<string> onListenTransactionFailed = null, CancellationToken cancellationToken = default)
        {
            UnityWebRequest request = null;
            
            try
            {
                var url = $"{firebaseSettings.GetDatabaseBaseUrl()}/{firebaseSettings.MessagesEndpoint}.json";
                request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
                request.SetRequestHeader("Accept", "text/event-stream");
                
                var sseDownloadHandler = new RealtimeDatabase_SSEDownloadHandler();
                sseDownloadHandler.onPut = onPut;
                sseDownloadHandler.onPatch = onPatch;
                request.downloadHandler = sseDownloadHandler;

                Debug.Log("Listening to " + url);
                var requestOperation = request.SendWebRequest();
                while (!requestOperation.isDone)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    
                    await Task.Yield();
                }
                Debug.Log("Stop listening to " + url);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                if (request != null)
                    request.Dispose();
            }
        }
    }
    
    [Serializable]
    public class Message
    {
        public string author;
        public string message;
        public string imageUrl;
        public bool isApproved;
        public long createdAt;
    }
}
