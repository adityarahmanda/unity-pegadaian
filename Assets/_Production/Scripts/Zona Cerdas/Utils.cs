using System;
using System.Collections.Generic;

namespace LZY.Pegadaian.ZonaCerdas
{
    public static class Utils
    {
        public static Message CreateMessage(Dictionary<string, object> newMessageData)
        {
            var messageData = new Message();
            foreach (var (fieldName, fieldValue) in newMessageData)
                SetMessageField(messageData, fieldName, fieldValue);
            return messageData;
       }
        
        public static void UpdateMessage(Message message, Dictionary<string, object> newMessageData)
        {
            foreach (var (fieldName, fieldValue) in newMessageData)
                SetMessageField(message, fieldName, fieldValue);
        }
        
        public static void SetMessageField(Message message, string fieldName, object fieldValue)
        {
            switch (fieldName)
            {
                case "author":
                    message.author = fieldValue.ToString();
                    break;
                case "message":
                    message.message = fieldValue.ToString();
                    break;
                case "imageUrl":
                    message.imageUrl = fieldValue.ToString();
                    break;
                case "isApproved":
                    message.isApproved = (bool)fieldValue;
                    break;
                case "createdAt":
                    message.createdAt = Convert.ToInt64(fieldValue);
                    break;
            }
        }
    }
}