using UnityEngine;

namespace LZY.Pegadaian.Firebase
{
    [CreateAssetMenu(fileName = "FirebaseSettings", menuName = "InteractiveMessage/Firebase Settings", order = 1)]
    public class FirebaseSettings : ScriptableObject
    {
        public string APIKey;
        public bool IsProduction = false;
    
        [Header("Realtime Database Settings")]
        public string ProductionDatabaseBaseUrl;
        public string SandboxDatabaseBaseUrl;

        [Header("Realtime Database Endpoints")]
        public string MessagesEndpoint;

        public string GetDatabaseBaseUrl()
        {
            return IsProduction ? ProductionDatabaseBaseUrl : SandboxDatabaseBaseUrl;
        }
    }
}