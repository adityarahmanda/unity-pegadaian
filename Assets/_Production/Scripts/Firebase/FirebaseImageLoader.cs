using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace LZY.Pegadaian.Firebase
{
    public static class FirebaseImageLoader
    {
        private static readonly int _maxImageCacheSize = 100;
        private static readonly Dictionary<string, Texture2D> _imageCacheDictionary = new();
        private static readonly LinkedList<string> _imageCacheOrder = new();
        
        public static Texture2D GetCachedImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return null;

            if (_imageCacheDictionary.TryGetValue(imageUrl, out var cachedTexture))
            {
                _imageCacheOrder.Remove(imageUrl);
                _imageCacheOrder.AddLast(imageUrl);
                return cachedTexture;
            }

            return null;
        }
        
        public static async UniTask<Texture2D> FetchImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return null;

            Debug.Log("Getting cached image: " + imageUrl);
            var cachedImage = GetCachedImage(imageUrl);
            if (cachedImage != null)
            {
                Debug.Log("Cached image found: " + imageUrl);
                return cachedImage;
            }

            try
            {
                Debug.Log("Downloading image: " + imageUrl);
                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
                {
                    await request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        var texture = DownloadHandlerTexture.GetContent(request);

                        _imageCacheDictionary[imageUrl] = texture;
                        _imageCacheOrder.AddLast(imageUrl);

                        if (_imageCacheOrder.Count > _maxImageCacheSize)
                        {
                            var oldestUrl = _imageCacheOrder.First.Value;
                            _imageCacheOrder.RemoveFirst();

                            if (_imageCacheDictionary.TryGetValue(oldestUrl, out var oldTexture))
                                Object.Destroy(oldTexture);

                            _imageCacheDictionary.Remove(oldestUrl);
                        }

                        Debug.Log("Image downloaded: " + imageUrl);
                        return texture;
                    }
                    else
                    {
                        Debug.LogError($"Image download failed: {imageUrl}, {request.error}");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return null;
        }
    }
}
