using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace LZY.Pegadaian.Firebase
{
    public class RealtimeDatabase_SSEDownloadHandler : SSEDownloadHandler
    {
        public RealtimeDatabase_SSEDownloadHandler() : base(new byte[1024]) { }

        private readonly string eventPrefix = "event: ";
        private readonly string dataPrefix = "data: ";
    
        public UnityAction<RealtimeDatabase_SSEData> onPut;
        public UnityAction<RealtimeDatabase_SSEData> onPatch;
    
        protected override void OnNewLineReceived(string line)
        {
            var sseEvent = string.Empty;
            var sseData = new RealtimeDatabase_SSEData();
            Debug.Log(line);

            var values = line.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var value in values)
            {
                if (value.StartsWith(eventPrefix))
                {
                    sseEvent = value.Substring(eventPrefix.Length).Trim();
                    if (sseEvent == "keep-alive") return;
                }
                else if (value.StartsWith(dataPrefix))
                {
                    var dataString = value.Substring(dataPrefix.Length).Trim('\r', '\n', '\t', ' ', '\0');
                    if (dataString == "null") return;
                    sseData = JsonConvert.DeserializeObject<RealtimeDatabase_SSEData>(dataString);
                }
            }

            switch (sseEvent)
            {
                case "put":
                    onPut?.Invoke(sseData);
                    break;
                case "patch":
                    onPatch?.Invoke(sseData);
                    break;
            }
        }
    } 

    public class RealtimeDatabase_SSEData
    {
        [JsonProperty("path")]
        public string Path;
        [JsonProperty("data")]
        public Dictionary<string, object> Data = new Dictionary<string, object>();
    }
}
