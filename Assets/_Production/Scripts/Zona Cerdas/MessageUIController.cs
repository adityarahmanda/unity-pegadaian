using System.Collections.Generic;
using LZY.Pegadaian.Firebase;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LZY.Pegadaian.ZonaCerdas
{
    public class MessageUIController : MonoBehaviour
    {
        public string Id => id;
        [Header("Read Only")]
        [SerializeField, ReadOnly] private string id;

        [SerializeField, ReadOnly] private Message message;

        [Header("Components")]
        [SerializeField] private RawImage rawImage;
        [SerializeField] private AspectRatioFitter imageAspectFitter;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI messageText;
        
        private string _imageUrl;

        public void SetMessageEmpty()
        {
            id = string.Empty;
            message = null;
            OnMessageUpdated();
        }

        public void SetMessage(string id, Message message)
        {
            this.id = id;
            this.message = message;
            OnMessageUpdated();
        }
        
        public void SetMessage(Dictionary<string, object> messageFieldDictionary)
        {
            Utils.UpdateMessage(message, messageFieldDictionary);
            OnMessageUpdated();
        }
        
        public void SetMessageField(string fieldName, object fieldValue)
        {
            Utils.SetMessageField(message, fieldName, fieldValue);
            OnMessageUpdated();
        }

        private void OnMessageUpdated()
        {
            if (message == null)
            {
                messageText.text = string.Empty;
                nameText.text = string.Empty;
                return;
            }
            
            messageText.text = message.message;
            nameText.text = message.author;
            UpdateImage(message.imageUrl);
        }

        private void UpdateImage(string imageUrl)
        {
            if (_imageUrl == imageUrl) return;
            
            var cachedImage = FirebaseImageLoader.GetCachedImage(imageUrl);
            rawImage.texture = cachedImage;
            if (cachedImage == null)
                imageAspectFitter.aspectRatio = 1f;
            else            
                imageAspectFitter.aspectRatio = cachedImage.width / (float)cachedImage.height;
        }
    }
}