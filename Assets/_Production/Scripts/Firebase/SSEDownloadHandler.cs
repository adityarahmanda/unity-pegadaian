using System.Text;
using UnityEngine.Networking;

namespace LZY.Pegadaian.Firebase
{
    public abstract class SSEDownloadHandler : DownloadHandlerScript
    {
        public string FullText => _text.ToString();

        public SSEDownloadHandler(byte[] buffer) : base(buffer)
        {
        }

        private StringBuilder _text = new StringBuilder();

        protected override bool ReceiveData(byte[] newData, int dataLength)
        {
            if (newData == null || newData.Length < 1) return false;

            var chunk = Encoding.UTF8.GetString(newData, 0, dataLength);
            _text.Append(chunk);

            int newlineIndex;
            while ((newlineIndex = _text.ToString().IndexOf("\n\n")) != -1)
            {
                var line = _text.ToString().Substring(0, newlineIndex).Trim();
                _text.Remove(0, newlineIndex + 2);
                OnNewLineReceived(line);
            }

            return true;
        }

        protected abstract void OnNewLineReceived(string line);
    }
}


