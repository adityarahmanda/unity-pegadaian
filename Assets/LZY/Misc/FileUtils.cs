using System.IO;
using UnityEngine;

namespace LZY.Utilities
{
    public static class FileUtils
    {
        public static Texture2D GetFileTexture(string path)
        {
            var imageBytes = ReadFileBytes(path);
            return GetFileTexture(imageBytes);
        }

        public static Texture2D GetFileTexture(byte[] imageBytes)
        {
            if (imageBytes == null) return null;

            var texture = new Texture2D(2, 2);
            return texture.LoadImage(imageBytes) ? texture : null;
        }

        public static void WriteFileBytes(string path, byte[] bytes)
        {
            if (string.IsNullOrEmpty(path)) return;

            var directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory)) return;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllBytes(path, bytes);
            Debug.Log("File saved at: " + path);
        }

        public static byte[] ReadFileBytes(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (!File.Exists(path))
            {
                Debug.LogWarning("File not found: " + path);
                return null;
            }

            try
            {
                byte[] data = File.ReadAllBytes(path);
                if (data.Length == 0)
                {
                    Debug.LogWarning("File is empty: " + path);
                    return null;
                }

                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading file: " + e.Message);
                return null;
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"File deleted: {path}");
            }
            else
            {
                Debug.LogWarning($"File not found: {path}");
            }
        }
    }
}
