using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityWebSocket;

namespace LZY.Resolume
{
    public class ResolumeWebSocket
    {
        public static string WebSocketURL = "ws://localhost:8080/api/v1";
        
        private WebSocket _webSocket;
        private Action<OpenEventArgs> _onOpen;
        private Action<CloseEventArgs> _onClose;
        private Action<ErrorEventArgs> _onError;
        private Action<MessageEventArgs> _onMessage;
        
        public ResolumeWebSocket(Action<OpenEventArgs> onOpen, Action<CloseEventArgs> onClose, Action<ErrorEventArgs> onError, Action<MessageEventArgs> onMessage)
        {
            _onOpen = onOpen;
            _onClose = onClose;
            _onError = onError;
            _onMessage = onMessage;
            
            _webSocket = new WebSocket(WebSocketURL);
            _webSocket.OnOpen += OnOpen;
            _webSocket.OnClose += OnClose;
            _webSocket.OnError += OnError;
            _webSocket.OnMessage += OnMessage;
        }

        public void ConnectAsync()
        {
            _webSocket?.ConnectAsync();
        }
        
        public void CloseAsync()
        {
            _webSocket?.CloseAsync();
        }
        
        private void OnOpen(object sender, OpenEventArgs e)
        {
            Debug.Log("[ResolumeWebSocket] Resolume Web Socket Open");
            _onOpen?.Invoke(e);
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            _onMessage?.Invoke(e);
        }
        
        private void OnError(object sender, ErrorEventArgs ex)
        {
            Debug.LogError("[ResolumeWebSocket] " + ex.Message);
            _onError?.Invoke(ex);
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            Debug.Log("[ResolumeWebSocket] Web socket closed");
            _onClose?.Invoke(e);
        }
        
        public void ConnectColumn(int column)
        {
            var data = new WebSocketData()
            {
                action = WebSocketAction.trigger,
                path = $"/composition/columns/{column}",
                parameter = $"/composition/columns/{column}/connected"
            };
            
            var json = JsonConvert.SerializeObject(data);
            _webSocket.SendAsync(json);
        }
        
        public void ConnectClip(ClipRawData videoData)
        {
            var data = new WebSocketData()
            {
                action = WebSocketAction.trigger,
                path = videoData.GetVideoPath(),
                parameter = videoData.GetVideoPath() + "/connected"
            };
            
            var json = JsonConvert.SerializeObject(data);
            _webSocket.SendAsync(json);
        }
        
        public void SubscribeClipConnectedParameter(ClipRawData videoData)
        {
            var data = new WebSocketData()
            {
                action = WebSocketAction.subscribe,
                path = videoData.GetVideoPath(),
                parameter = videoData.GetVideoPath() + "/connected",
            };
            var json = JsonConvert.SerializeObject(data);
            _webSocket.SendAsync(json);
        }
    }
}