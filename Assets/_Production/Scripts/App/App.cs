using UnityEngine;

namespace LZY.AppName
{
    public class App : MonoBehaviour
    {
        // Put all Manager that shared between Scenes.
        // For Example, GameDatabase, BackendApi, etc.
        // This script is DDOL.
        
        public static App Instance { get; private set; }

        private bool isInitialized = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(this);

    #if UNITY_SERVER
            Application.targetFrameRate = 30;
    #else
            Application.targetFrameRate = 60;
    #endif

            Initialize();
        }

        public void Initialize()
        {
            if (isInitialized) return;
            isInitialized = true;

            // Initialize all manager here.
        }
    }
}
