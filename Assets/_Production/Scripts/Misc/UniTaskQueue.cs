using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace LZY.Pegadaian.Misc
{
    [Serializable]
    public class UniTaskQueue : IDisposable
    {
        private Queue<Func<CancellationToken, UniTask>> _taskQueue = new Queue<Func<CancellationToken, UniTask>>();
        private float _queueDelay;
        private bool _isProcessingTask;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isDisposed = false;

        public UniTaskQueue()
        {
        }

        public UniTaskQueue(float queueDelay)
        {
            _queueDelay = queueDelay;
        }

        public void QueueTask(Func<CancellationToken, UniTask> task)
        {
            if (_isDisposed) return;

            _taskQueue.Enqueue(task);
            if (!_isProcessingTask)
            {
                _isProcessingTask = true;
                QueueProcessTaskAsync(_cts.Token).Forget();
            }
        }

        private async UniTaskVoid QueueProcessTaskAsync(CancellationToken token)
        {
            try
            {
                while (_taskQueue.Count > 0)
                {
                    if (token.IsCancellationRequested)
                        break;

                    var taskFunction = _taskQueue.Dequeue();
                    await taskFunction(token);
                    await UniTask.WaitForSeconds(_queueDelay, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected on disposal
            }
            finally
            {
                _isProcessingTask = false;
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;
            _cts.Cancel();
            _cts.Dispose();
            _taskQueue.Clear();
        }
    }
}