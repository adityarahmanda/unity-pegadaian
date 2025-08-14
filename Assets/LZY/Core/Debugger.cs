using UnityEngine;

namespace LZY
{
    public static class Debugger
    {
        private static bool debugEnabled = false;

        public static void Log(object message)
        {
            if (debugEnabled)
            {
                Debug.Log(message);
            }
        }

        public static void LogColor(object message, Color color)
        {
            if (debugEnabled)
            {
                Debug.Log("<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + message + "</color>");
            }
        }

        public static void LogWarning(object message)
        {
            if (debugEnabled)
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogError(object message)
        {
            if (debugEnabled)
            {
                Debug.LogError(message);
            }
        }

        public static void EnableDebug()
        {
            debugEnabled = true;
            Debug.Log("Debug mode enabled.");
        }

        public static void DisableDebug()
        {
            debugEnabled = false;
            Debug.Log("Debug mode disabled.");
        }
    }
}
